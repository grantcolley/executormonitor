using DevelopmentInProgress.DipCore;
using DevelopmentInProgress.ExecutorMonitor.Wpf.Model;
using DevelopmentInProgress.ExecutorMonitor.Wpf.Services;
using DevelopmentInProgress.Origin.Context;
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
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.ViewModel
{
    public class MonitorViewModel : DocumentViewModel
    {
        private readonly IMonitorService monitorService;
        private ObservableCollection<Message> notifications;
        private HubConnection hubConnection;
        private Run selectedRun;
        private bool isMonitorEnabled;
        private bool isExecuteRunEnabled;
        private bool hasSubscribed;
        private IList<RunStep> notificationSteps;

        public MonitorViewModel(ViewModelContext viewModelContext, MonitorService monitorService)
            : base(viewModelContext)
        {
            MonitorCommand = new ViewModelCommand(Monitor);
            ExecuteRunCommand = new ViewModelCommand(ExecuteRun);
            DisconnectCommand = new ViewModelCommand(Disconnect);
            ClearNotificationsCommand = new ViewModelCommand(ClearNotifications);

            notifications = new ObservableCollection<Message>();

            this.monitorService = monitorService;
        }

        public ICommand ExecuteRunCommand { get; set; }
        public ICommand MonitorCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }
        public ICommand ClearNotificationsCommand { get; set; }
        public List<Run> Runs { get; set; }

        public ObservableCollection<Message> Notifications
        {
            get { return notifications; }
        }

        public Run SelectedRun
        {
            get { return selectedRun; }
            set
            {
                if (selectedRun != value)
                {
                    selectedRun = value;

                    IsExecuteRunEnabled = (selectedRun != null);
                    IsMonitorEnabled = (selectedRun != null);

                    OnPropertyChanged("SelectedRun");
                    OnPropertyChanged("Steps");
                }
            }
        }

        public bool IsExecuteRunEnabled
        {
            get { return isExecuteRunEnabled; }
            set
            {
                if (isExecuteRunEnabled != value)
                {
                    isExecuteRunEnabled = value;
                    OnPropertyChanged("IsExecuteRunEnabled");
                }
            }
        }
        
        public bool IsMonitorEnabled
        {
            get { return isMonitorEnabled; }
            set
            {
                if (isMonitorEnabled != value)
                {
                    isMonitorEnabled = value;
                    OnPropertyChanged("IsMonitorEnabled");
                }
            }
        }

        public bool HasNotSubscribed
        {
            get { return !hasSubscribed; }
        }

        public bool HasSubscribed
        {
            get { return hasSubscribed; }
            set
            {
                if (hasSubscribed != value)
                {
                    hasSubscribed = value;
                    if(hasSubscribed)
                    {
                        IsMonitorEnabled = false;
                        IsExecuteRunEnabled = false;
                    }

                    OnPropertyChanged("HasSubscribed");
                    OnPropertyChanged("HasNotSubscribed");
                }
            }
        }

        protected async override void OnPublished(object data)
        {
            Runs = await monitorService.GetRuns();
            Reset();
        }

        protected async override void SaveDocument()
        {
            // Save stuff here...
        }

        protected async override void OnDisposing()
        {
            await Disconnect();
        }

        private void Disconnect(object param)
        {
            Reset();
        }

        private async Task Disconnect()
        {
            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        private async void Monitor(object param)
        {
            await Monitor();
        }

        private async Task<bool> Monitor()
        {
            if (SelectedRun == null)
            {
                ShowMessage(new Message { MessageType = MessageType.Info, Text = "Select a Run to minitor" });
                return false;
            }

            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{SelectedRun.NotificationUrl}/notificationhub?runid={SelectedRun.RunId}")
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

                notificationSteps = SelectedRun.RunStep.Flatten<RunStep>(r => r.RunId.Equals(SelectedRun.RunId)).ToList();

                HasSubscribed = true;

                return HasSubscribed;
            }
            catch(Exception ex)
            {
                OnConnected(new Message { MessageType = MessageType.Error, Text = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} Failed to connect", Timestamp = DateTime.Now });
                throw;
            }
        }

        public async void ClearNotifications(object param)
        {
            Reset();
        }

        private async void Reset()
        {
            await Disconnect();
            SelectedRun = null;
            HasSubscribed = false;
            Notifications.Clear();
            ClearMessages();
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
                var step = notificationSteps.First(s=>s.StepId.Equals(stepNotification.StepId));
                step.Status = stepNotification.Status;
                step.Message = $"{stepNotification.Timestamp.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} {stepNotification.Message}";

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

        private async void ExecuteRun(object param)
        {
            try
            {
                var result = await Monitor();

                if (result)
                {
                    var jsonContent = JsonConvert.SerializeObject(SelectedRun.RunStep.Step);
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await client.PostAsync(SelectedRun.RunStep.StepUrl, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
                    }
                }
            }
            catch(Exception ex)
            {
                ShowMessage(new Message { MessageType = MessageType.Error, Text = ex.Message });
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
    }
}