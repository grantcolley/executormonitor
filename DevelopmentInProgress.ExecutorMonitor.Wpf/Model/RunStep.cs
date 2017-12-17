using DevelopmentInProgress.DipCore;
using DipRunner;
using System.Collections.Generic;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Model
{
    public class RunStep : EntityBase
    {
        private Step step;
        private string message;

        public RunStep(Step step)
        {
            this.step = step;
            SubSteps = new List<RunStep>();
            TransitionSteps = new List<RunStep>();
        }
        
        public int RunId
        {
            get { return step.RunId; }
        }

        public int StepId
        {
            get { return step.StepId; }
        }

        public string StepName
        {
            get { return step.StepName; }
        }

        public string TargetType
        {
            get { return step.TargetType; }
        }
        
        public string TargetAssembly
        {
            get { return step.TargetAssembly; }
        }

        public string StepUrl
        {
            get { return step.StepUrl; }
        }

        public Step Step
        {
            get { return step; }
        }

        public List<RunStep> SubSteps { get; private set; }

        public List<RunStep> TransitionSteps { get; private set; }

        public StepStatus Status
        {
            get { return step.Status; }
            set
            {
                if (step.Status != value)
                {
                    step.Status = value;
                    OnPropertyChanged("Status");
                    OnPropertyChanged("Tooltip");
                }
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged("Message");
                    OnPropertyChanged("Tooltip");
                }
            }
        }

        public string ToolTip
        {
            get { return $"StepId: {StepId}\nStepName: {StepName}\nStatus: {Status}\nTargetType: {TargetType}\nTargetAssembly: {TargetAssembly}\nStepUrl: {StepUrl}\nMessage: {Message}"; }
        }
    }
}