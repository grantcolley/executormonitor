using DevelopmentInProgress.DipCore;
using DevelopmentInProgress.WPFControls.Messaging;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Model
{
    public class Run : EntityBase
    {
        private bool isMonitorEnabled;
        private bool isExecuteRunEnabled;
        private bool hasConnected;
        private HubConnection hubConnection;
        private IList<RunStep> notificationSteps;
        private ObservableCollection<Message> notifications;

        public Run()
        {
            notifications = new ObservableCollection<Message>();

            IsMonitorEnabled = true;
            IsExecuteRunEnabled = true;
        }

        public int RunId { get; set; }
        public string RunName { get; set; }
        public string NotificationUrl{get;set;}
        public RunStep RunStep { get; set; }

        public HubConnection HubConnection
        {
            get { return hubConnection; }
            set { hubConnection = value; }
        }

        public IList<RunStep> NotificationSteps
        {
            get { return notificationSteps; }
            set { notificationSteps = value; }
        }

        public ObservableCollection<Message> Notifications
        {
            get { return notifications; }
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

        public bool HasNotConnected
        {
            get { return !hasConnected; }
        }

        public bool HasConnected
        {
            get { return hasConnected; }
            set
            {
                if (hasConnected != value)
                {
                    hasConnected = value;
                    if (hasConnected)
                    {
                        IsMonitorEnabled = false;
                        IsExecuteRunEnabled = false;
                    }
                    else
                    {
                        IsMonitorEnabled = true;
                        IsExecuteRunEnabled = true;
                    }

                    OnPropertyChanged("HasConnected");
                    OnPropertyChanged("HasNotConnected");
                }
            }
        }

        public void FlattenRootRunStep()
        {
            NotificationSteps = RunStep.Flatten<RunStep>(r => r.RunId.Equals(RunId)).ToList();
        }
    }
}