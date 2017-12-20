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
        private bool hasSubscribed;
        private HubConnection hubConnection;
        private IList<RunStep> notificationSteps;
        private ObservableCollection<Message> notifications;

        public Run()
        {
            notifications = new ObservableCollection<Message>();
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
                    if (hasSubscribed)
                    {
                        IsMonitorEnabled = false;
                        IsExecuteRunEnabled = false;
                    }

                    OnPropertyChanged("HasSubscribed");
                    OnPropertyChanged("HasNotSubscribed");
                }
            }
        }

        public void FlattenRootRunStep()
        {
            NotificationSteps = RunStep.Flatten<RunStep>(r => r.RunId.Equals(RunId)).ToList();
        }
    }
}