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
        private Run selectedRun;
        
        public MonitorViewModel(ViewModelContext viewModelContext, MonitorService monitorService)
            : base(viewModelContext)
        {
            MonitorCommand = new ViewModelCommand(Monitor);
            ExecuteRunCommand = new ViewModelCommand(ExecuteRun);
            DisconnectCommand = new ViewModelCommand(Disconnect);
            ClearNotificationsCommand = new ViewModelCommand(ClearNotifications);

            this.monitorService = monitorService;
        }

        public ICommand ExecuteRunCommand { get; set; }
        public ICommand MonitorCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }
        public ICommand ClearNotificationsCommand { get; set; }
        public List<Run> Runs { get; set; }

        public Run SelectedRun
        {
            get { return selectedRun; }
            set
            {
                if (selectedRun != value)
                {
                    selectedRun = value;

                    OnPropertyChanged("SelectedRun");
                    OnPropertyChanged("SelectedRunName");
                    OnPropertyChanged("IsRunSelected");
                }
            }
        }

        public string SelectedRunName => selectedRun == null ? null : $"{selectedRun.RunName} - {selectedRun.RunId}";

        public bool IsRunSelected => selectedRun == null ? false : true;

        protected async override void OnPublished(object data)
        {
            SelectedRun = null;
            if (Runs == null
                || !Runs.Any())
            {
                Runs = await monitorService.GetRuns();
            }
        }

        protected async override void SaveDocument()
        {
            // Save stuff here...
        }

        protected async override void OnDisposing()
        {
            foreach(var run in Runs)
            {
                await Disconnect(run);
            }
        }

        private void Disconnect(object param)
        {
            var run = param as Run;
            Reset(run);
        }

        private async Task Disconnect(Run run)
        {
            if (run.HubConnection != null)
            {
                await run.HubConnection.DisposeAsync();
                run.HubConnection = null;
                run.HasConnected = false;
            }
        }

        private async void Monitor(object param)
        {
            var run = param as Run;
            await Monitor(run);
        }

        private async Task<bool> Monitor(Run run)
        {
            if (run == null)
            {
                ShowMessage(new Message { MessageType = MessageType.Info, Text = "Select a Run to minitor" });
                return false;
            }

            run.HubConnection = new HubConnectionBuilder()
                .WithUrl($"{run.NotificationUrl}/notificationhub?runid={run.RunId}")
                .WithTransport(TransportType.WebSockets)
                .Build();

            run.HubConnection.On<object>("Connected", message =>
            {
                ViewModelContext.UiDispatcher.Invoke(() =>
                {
                    OnConnected(new Message { MessageType = MessageType.Info, Text = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} {message.ToString()}", Timestamp = DateTime.Now });
                });
            });

            run.HubConnection.On<object>("Send", (message) =>
            {
                ViewModelContext.UiDispatcher.Invoke(() =>
                {
                    OnNotificationRecieved(message);
                });
            });

            try
            {
                await run.HubConnection.StartAsync();
                
                run.HasConnected = true;

                return run.HasConnected;
            }
            catch(Exception)
            {
                OnConnected(new Message { MessageType = MessageType.Error, Text = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} Failed to connect", Timestamp = DateTime.Now });
                throw;
            }
        }

        public async void ClearNotifications(object param)
        {
            var run = param as Run;
            Reset(run);
        }

        private async void Reset(Run run)
        {
            if (run != null)
            {
                await Disconnect(run);
                run.Notifications.Clear();
                //TODO: clear RunStep messages
                ClearMessages();
            }
        }

        private void OnConnected(Message message)
        {
            SelectedRun.Notifications.Add(message);
            OnPropertyChanged("Notifications");
        }

        private void OnNotificationRecieved(object message)
        {
            var stepNotifications = JsonConvert.DeserializeObject<IEnumerable<StepNotification>>(message.ToString()).ToList();
            foreach (var stepNotification in stepNotifications)
            {
                var run = Runs.FirstOrDefault(r => r.RunId == stepNotification.RunId);
                if (run == null)
                {
                    continue;
                }

                var step = run.NotificationSteps.First(s=>s.StepId.Equals(stepNotification.StepId));
                step.Status = stepNotification.Status;
                step.Message = $"{stepNotification.Timestamp.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} {stepNotification.Status} {stepNotification.Message}";

                var msg = new Message
                {
                    MessageType = NotificationLevelToMessageTypeConverter(stepNotification.NotificationLevel),
                    Text = $"{stepNotification.Timestamp.ToString("dd/MM/yyyy hh:mm:ss.fff tt")} {stepNotification.StepId} {stepNotification.StepName} {stepNotification.Status} {stepNotification.Message}",
                    Timestamp = stepNotification.Timestamp,
                    TextVerbose = stepNotification.ToString()
                };

                var indexedNotification = run.Notifications.FirstOrDefault(n => n.Timestamp.Ticks > msg.Timestamp.Ticks);
                if (indexedNotification != null)
                {
                    var index = run.Notifications.IndexOf(indexedNotification);
                    run.Notifications.Insert(index, msg);
                }
                else
                {
                    run.Notifications.Add(msg);
                }
            }
        }

        private async void ExecuteRun(object param)
        {
            try
            {
                var run = param as Run;

                var result = await Monitor(run);

                if (result)
                {
                    var jsonContent = JsonConvert.SerializeObject(run.RunStep.Step);
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await client.PostAsync(run.RunStep.StepUrl, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
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