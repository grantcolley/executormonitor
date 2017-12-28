using DevelopmentInProgress.ExecutorMonitor.Wpf.Model;
using DipRunner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Services
{
    public class MonitorService : IMonitorService
    {
        public Task<List<Run>> GetRuns()
        {
            var tcs = new TaskCompletionSource<List<Run>>();

            var run1 = new Run { RunId = 101, RunName = "Test Run", NotificationUrl = "http://localhost:5000" };
            var step1 = GetRunSteps1(run1.RunId, run1.RunName);
            run1.RunStep = step1;

            var run2 = new Run { RunId = 201, RunName = "2nd Run", NotificationUrl = "http://localhost:5000" };
            var step2 = GetRunSteps2(run2.RunId, run2.RunName);
            run2.RunStep = step2;

            var runs = new List<Run>() { run1, run2};
            tcs.SetResult(runs);
            return tcs.Task;
        }

        private RunStep GetRunSteps1(int runId, string runName)
        {
            var step1 = new Step();
            step1.RunId = runId;
            step1.RunName = runName;
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

            var step2 = new Step();
            step2.RunId = runId;
            step2.RunName = runName;
            step2.StepId = 2;
            step2.StepName = "Step 2";

            var step21 = new Step();
            step21.RunId = runId;
            step21.RunName = runName;
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
            step22.RunId = runId;
            step22.RunName = runName;
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
            step23.RunId = runId;
            step23.RunName = runName;
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
            step3.RunId = runId;
            step3.RunName = runName;
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
            step31.RunId = runId;
            step31.RunName = runName;
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
            step4.RunId = runId;
            step4.RunName = runName;
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

            step1.TransitionSteps = new Step[] { step2 };
            step2.SubSteps = new Step[] { step21, step22, step23 };
            step2.TransitionSteps = new Step[] { step3 };
            step3.SubSteps = new Step[] { step31 };
            step3.TransitionSteps = new Step[] { step4 };

            var runStep1 = new RunStep(step1);
            var runStep2 = new RunStep(step2);
            var runStep21 = new RunStep(step21);
            var runStep22 = new RunStep(step22);
            var runStep23 = new RunStep(step23);
            var runStep3 = new RunStep(step3);
            var runStep31 = new RunStep(step31);
            var runStep4 = new RunStep(step4);

            runStep1.TransitionSteps.Add(runStep2);
            runStep2.SubSteps.AddRange(new[] { runStep21, runStep22, runStep23 });
            runStep2.TransitionSteps.Add(runStep3);
            runStep3.SubSteps.Add(runStep31);
            runStep3.TransitionSteps.Add(runStep4);

            return runStep1;
        }

        private RunStep GetRunSteps2(int runId, string runName)
        {
            var step1 = new Step();
            step1.RunId = runId;
            step1.RunName = runName;
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

            var step2 = new Step();
            step2.RunId = runId;
            step2.RunName = runName;
            step2.StepId = 2;
            step2.StepName = "Step 2";

            var step21 = new Step();
            step21.RunId = runId;
            step21.RunName = runName;
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
            step22.RunId = runId;
            step22.RunName = runName;
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

            var step221 = new Step();
            step221.RunId = runId;
            step221.RunName = runName;
            step221.StepId = 221;
            step221.StepName = "Step 2.2.1";
            step221.TargetAssembly = "TestLibrary.dll";
            step221.TargetType = "TestLibrary.TestRunner";
            step221.Payload = "5000|Sub Step 2.2.1";
            step221.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"

            };

            var step222 = new Step();
            step222.RunId = runId;
            step222.RunName = runName;
            step222.StepId = 222;
            step222.StepName = "Step 2.2.2";
            step222.TargetAssembly = "TestLibrary.dll";
            step222.TargetType = "TestLibrary.TestRunner";
            step222.Payload = "5000|Sub Step 2.2.2";
            step222.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"

            };
            
            var step23 = new Step();
            step23.RunId = runId;
            step23.RunName = runName;
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

            var step24 = new Step();
            step24.RunId = runId;
            step24.RunName = runName;
            step24.StepId = 24;
            step24.StepName = "Step 2.4";
            step24.TargetAssembly = "TestLibrary.dll";
            step24.TargetType = "TestLibrary.TestRunner";
            step24.Payload = "2000|Goodbye";
            step24.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step3 = new Step();
            step3.RunId = runId;
            step3.RunName = runName;
            step3.StepId = 3;
            step3.StepName = "Step 3";
            step3.TargetAssembly = "TestLibrary.dll";
            step3.TargetType = "TestLibrary.TestRunner";
            step3.Payload = "1500|Goodbye";
            step3.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var step4 = new Step();
            step4.RunId = runId;
            step4.RunName = runName;
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

            step1.TransitionSteps = new Step[] { step2 };
            step2.SubSteps = new Step[] { step21, step22, step23, step24 };
            step2.TransitionSteps = new Step[] { step3 };

            var runStep1 = new RunStep(step1);
            var runStep2 = new RunStep(step2);
            var runStep21 = new RunStep(step21);
            var runStep22 = new RunStep(step22);
            var runStep221 = new RunStep(step221);
            var runStep222 = new RunStep(step222);
            var runStep23 = new RunStep(step23);
            var runStep24 = new RunStep(step24);
            var runStep3 = new RunStep(step3);
            var runStep4 = new RunStep(step4);

            runStep1.TransitionSteps.Add(runStep2);

            runStep22.SubSteps.AddRange(new[] { runStep221, runStep222 });

            runStep2.SubSteps.AddRange(new[] { runStep21, runStep22, runStep23 });

            runStep2.TransitionSteps.Add(runStep3);
            runStep3.TransitionSteps.Add(runStep4);

            return runStep1;
        }
    }
}
