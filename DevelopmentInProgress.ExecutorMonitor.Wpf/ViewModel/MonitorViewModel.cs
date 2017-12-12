﻿using DevelopmentInProgress.Origin.Context;
using DevelopmentInProgress.Origin.ViewModel;
using DevelopmentInProgress.WPFControls.Messaging;
using DipRunner;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.ViewModel
{
    public class MonitorViewModel : DocumentViewModel
    {
        private ObservableCollection<Message> notifications;
        private HubConnection hubConnection;
        
        public MonitorViewModel(ViewModelContext viewModelContext)
            : base(viewModelContext)
        {
            SubscribeCommand = new ViewModelCommand(Subscribe);
            RunCommand = new ViewModelCommand(Run);
            ClearNotificationsCommand = new ViewModelCommand(ClearNotifications);

            notifications = new ObservableCollection<Message>();

            ServerUri = @"http://localhost:5000";
            RunId = "101";
        }

        public ICommand RunCommand { get; set; }
        public ICommand SubscribeCommand { get; set; }
        public ICommand ClearNotificationsCommand { get; set; }
        public string ServerUri { get; set; }
        public string RunId { get; set; }

        public ObservableCollection<Message> Notifications
        {
            get { return notifications; }
        }

        protected async override void OnPublished(object data)
        {
            // Do stuff here...
        }

        protected async override void SaveDocument()
        {
            // Save stuff here...
        }

        private async void Subscribe(object param)
        {
            ClearNotifications(null);

            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{ServerUri}/notificationhub?runid={RunId}")
                .WithTransport(TransportType.WebSockets)
                .Build();

            hubConnection.On<object>("Connected", message =>
            {
                ViewModelContext.UiDispatcher.Invoke(() =>
                {
                    OnConnected(new Message { MessageType = MessageType.Info, Text = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} {message.ToString()}", Timestamp = DateTime.Now });
                });
            });

            hubConnection.On<object>("Send", (message) =>
            {
                ViewModelContext.UiDispatcher.Invoke(() =>
                {
                    OnNotificationRecieved(message);
                });
            });

            try
            {
                await hubConnection.StartAsync();
            }
            catch(Exception ex)
            {
                OnConnected(new Message { MessageType = MessageType.Error, Text = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} Failed to connect", Timestamp = DateTime.Now });
            }
        }

        public void ClearNotifications(object param)
        {
            Notifications.Clear();
        }

        private void OnConnected(Message message)
        {
            Notifications.Add(message);
            OnPropertyChanged("Notifications");
        }

        private void OnNotificationRecieved(object message)
        {
            var stepNotifications = JsonConvert.DeserializeObject<IEnumerable<StepNotification>>(message.ToString()).ToList();
            foreach (var stepNotification in stepNotifications)
            {
                var msg = new Message
                {
                    MessageType = NotificationLevelToMessageTypeConverter(stepNotification.NotificationLevel),
                    Text = $"{stepNotification.Timestamp.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} {stepNotification.StepId} {stepNotification.StepName} {stepNotification.Status} {stepNotification.Message}",
                    Timestamp = stepNotification.Timestamp,
                    TextVerbose = stepNotification.ToString()
                };

                var indexedNotification = notifications.FirstOrDefault(n => n.Timestamp.Ticks > msg.Timestamp.Ticks);
                if (indexedNotification != null)
                {
                    var index = Notifications.IndexOf(indexedNotification);
                    Notifications.Insert(index, msg);
                }
                else
                {
                    Notifications.Add(msg);
                }
            }
        }

        private async void Run(object param)
        {
            var step = GetRoot();

            var jsonContent = JsonConvert.SerializeObject(step);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(step.StepUrl, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            }
        }

        private MessageType NotificationLevelToMessageTypeConverter(NotificationLevel notificationLevel)
        {
            switch (notificationLevel)
            {
                case NotificationLevel.Error:
                    return MessageType.Error;
                case NotificationLevel.Warning:
                    return MessageType.Warn;
                default:
                    return MessageType.Info;
            }
        }

        private Step GetRoot()
        {
            var step1 = new Step();
            step1.RunId = 101;
            step1.RunName = "Test Run 1";
            step1.StepId = 1;
            step1.StepName = "Step 1";
            step1.TargetAssembly = "TestLibrary.dll";
            step1.TargetType = "TestLibrary.TestRunner";
            step1.Payload = "1000|Hello";
            step1.Urls = new[] { "http://localhost:5000" };
            step1.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step21 = new Step();
            step21.RunId = 101;
            step21.RunName = "Test Run 1";
            step21.StepId = 21;
            step21.StepName = "Step 2.1";
            step21.TargetAssembly = "TestLibrary.dll";
            step21.TargetType = "TestLibrary.TestRunner";
            step21.Payload = "3000|Sub Step 2.1";
            step21.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step22 = new Step();
            step22.RunId = 101;
            step22.RunName = "Test Run 1";
            step22.StepId = 22;
            step22.StepName = "Step 2.2";
            step22.TargetAssembly = "TestLibrary.dll";
            step22.TargetType = "TestLibrary.TestRunner";
            step22.Payload = "5000|Sub Step 2.2";
            step22.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step23 = new Step();
            step23.RunId = 101;
            step23.RunName = "Test Run 1";
            step23.StepId = 23;
            step23.StepName = "Step 2.3";
            step23.TargetAssembly = "TestLibrary.dll";
            step23.TargetType = "TestLibrary.TestRunner";
            step23.Payload = "1000|Sub Step 2.3";
            step23.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step3 = new Step();
            step3.RunId = 101;
            step3.RunName = "Test Run 1";
            step3.StepId = 3;
            step3.StepName = "Step 3";
            step3.TargetAssembly = "TestLibrary.dll";
            step3.TargetType = "TestLibrary.TestRunner";
            step3.Payload = "2000|Goodbye";
            step3.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step31 = new Step();
            step31.RunId = 101;
            step31.RunName = "Test Run 1";
            step31.StepId = 31;
            step31.StepName = "Step 3.1";
            step31.TargetAssembly = "TestLibrary.dll";
            step31.TargetType = "TestLibrary.TestRunner";
            step31.Payload = "1500|Goodbye";
            step31.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step4 = new Step();
            step4.RunId = 101;
            step4.RunName = "Test Run 1";
            step4.StepId = 4;
            step4.StepName = "Step 4";
            step4.TargetAssembly = "TestLibrary.dll";
            step4.TargetType = "TestLibrary.TestRunner";
            step4.Payload = "7000|Goodbye";
            step4.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            step1.SubSteps = new[] { step21, step22, step23 };

            step3.SubSteps = new[] { step31 };

            step1.TransitionSteps = new[] { step3, step4 };

            return step1;
        }
    }
}