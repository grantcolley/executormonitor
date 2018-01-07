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

            var ifrs9 = new Run { RunId = 101, RunName = "IFRS9", NotificationUrl = "http://localhost:5000" };
            var ifrs9Root = GetIfrs9(ifrs9.RunId, ifrs9.RunName);
            ifrs9.RunStep = ifrs9Root;
            ifrs9.FlattenRootRunStep();

            var letterGeneration = new Run { RunId = 201, RunName = "Letter Generation", NotificationUrl = "http://localhost:5000" };
            var getLetters = GetLetters(letterGeneration.RunId, letterGeneration.RunName);
            letterGeneration.RunStep = getLetters;
            letterGeneration.FlattenRootRunStep();

            var runs = new List<Run>() { ifrs9, letterGeneration};
            tcs.SetResult(runs);
            return tcs.Task;
        }

        private RunStep GetIfrs9(int runId, string runName)
        {
            var ifrs9 = new Step();
            ifrs9.RunId = runId;
            ifrs9.RunName = runName;
            ifrs9.StepId = 1;
            ifrs9.StepName = "IFRS9";
            ifrs9.TargetAssembly = "TestLibrary.dll";
            ifrs9.TargetType = "TestLibrary.TestRunner";
            ifrs9.Payload = "1000|Hello";
            ifrs9.Urls = new[] { "http://localhost:5000" };
            ifrs9.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var sourceData = new Step();
            sourceData.RunId = runId;
            sourceData.RunName = runName;
            sourceData.StepId = 2;
            sourceData.StepName = "Source Data";

            var counterparties = new Step();
            counterparties.RunId = runId;
            counterparties.RunName = runName;
            counterparties.StepId = 21;
            counterparties.StepName = "Counterparties";
            counterparties.TargetAssembly = "TestLibrary.dll";
            counterparties.TargetType = "TestLibrary.TestRunner";
            counterparties.Payload = "3000|Sub Step 2.1";
            counterparties.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var pd = new Step();
            pd.RunId = runId;
            pd.RunName = runName;
            pd.StepId = 22;
            pd.StepName = "Probability Of Default";
            pd.TargetAssembly = "TestLibrary.dll";
            pd.TargetType = "TestLibrary.TestRunner";
            pd.Payload = "5000|Sub Step 2.2";
            pd.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var lgd = new Step();
            lgd.RunId = runId;
            lgd.RunName = runName;
            lgd.StepId = 23;
            lgd.StepName = "Loss Given Default";
            lgd.TargetAssembly = "TestLibrary.dll";
            lgd.TargetType = "TestLibrary.TestRunner";
            lgd.Payload = "1000|Sub Step 2.3";
            lgd.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var transform = new Step();
            transform.RunId = runId;
            transform.RunName = runName;
            transform.StepId = 3;
            transform.StepName = "Transform";
            transform.TargetAssembly = "TestLibrary.dll";
            transform.TargetType = "TestLibrary.TestRunner";
            transform.Payload = "2000|Goodbye";
            transform.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var applyPd = new Step();
            applyPd.RunId = runId;
            applyPd.RunName = runName;
            applyPd.StepId = 31;
            applyPd.StepName = "Apply PD's";
            applyPd.TargetAssembly = "TestLibrary.dll";
            applyPd.TargetType = "TestLibrary.TestRunner";
            applyPd.Payload = "1500|Goodbye";
            applyPd.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var applyLgd = new Step();
            applyLgd.RunId = runId;
            applyLgd.RunName = runName;
            applyLgd.StepId = 311;
            applyLgd.StepName = "Apply LGD's";
            applyLgd.TargetAssembly = "TestLibrary.dll";
            applyLgd.TargetType = "TestLibrary.TestRunner";
            applyLgd.Payload = "1500|Goodbye";
            applyLgd.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var modelling = new Step();
            modelling.RunId = runId;
            modelling.RunName = runName;
            modelling.StepId = 4;
            modelling.StepName = "Modelling";
            modelling.TargetAssembly = "TestLibrary.dll";
            modelling.TargetType = "TestLibrary.TestRunner";
            modelling.Payload = "7000|Goodbye";
            modelling.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };


            var scenario1 = new Step();
            scenario1.RunId = runId;
            scenario1.RunName = runName;
            scenario1.StepId = 41;
            scenario1.StepName = "Scenario 1";
            scenario1.TargetAssembly = "TestLibrary.dll";
            scenario1.TargetType = "TestLibrary.TestRunner";
            scenario1.Payload = "7000|Goodbye";
            scenario1.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var scenario2 = new Step();
            scenario2.RunId = runId;
            scenario2.RunName = runName;
            scenario2.StepId = 42;
            scenario2.StepName = "Scenario 2";
            scenario2.TargetAssembly = "TestLibrary.dll";
            scenario2.TargetType = "TestLibrary.TestRunner";
            scenario2.Payload = "7000|Goodbye";
            scenario2.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var scenario3 = new Step();
            scenario3.RunId = runId;
            scenario3.RunName = runName;
            scenario3.StepId = 43;
            scenario3.StepName = "Scenario 3";
            scenario3.TargetAssembly = "TestLibrary.dll";
            scenario3.TargetType = "TestLibrary.TestRunner";
            scenario3.Payload = "7000|Goodbye";
            scenario3.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var scenario4 = new Step();
            scenario4.RunId = runId;
            scenario4.RunName = runName;
            scenario4.StepId = 44;
            scenario4.StepName = "Scenario 4";
            scenario4.TargetAssembly = "TestLibrary.dll";
            scenario4.TargetType = "TestLibrary.TestRunner";
            scenario4.Payload = "7000|Goodbye";
            scenario4.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var scenario5 = new Step();
            scenario5.RunId = runId;
            scenario5.RunName = runName;
            scenario5.StepId = 45;
            scenario5.StepName = "Scenario 5";
            scenario5.TargetAssembly = "TestLibrary.dll";
            scenario5.TargetType = "TestLibrary.TestRunner";
            scenario5.Payload = "7000|Goodbye";
            scenario5.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var scenario6 = new Step();
            scenario6.RunId = runId;
            scenario6.RunName = runName;
            scenario6.StepId = 46;
            scenario6.StepName = "Scenario 6";
            scenario6.TargetAssembly = "TestLibrary.dll";
            scenario6.TargetType = "TestLibrary.TestRunner";
            scenario6.Payload = "7000|Goodbye";
            scenario6.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var reporting = new Step();
            reporting.RunId = runId;
            reporting.RunName = runName;
            reporting.StepId = 5;
            reporting.StepName = "Reporting";
            reporting.TargetAssembly = "TestLibrary.dll";
            reporting.TargetType = "TestLibrary.TestRunner";
            reporting.Payload = "7000|Goodbye";
            reporting.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            ifrs9.TransitionSteps = new Step[] { sourceData };
            sourceData.SubSteps = new Step[] { counterparties, pd, lgd };
            sourceData.TransitionSteps = new Step[] { transform };
            transform.SubSteps = new Step[] { applyPd };
            applyPd.SubSteps = new Step[] { applyLgd};
            transform.TransitionSteps = new Step[] { modelling };
            modelling.SubSteps = new Step[] { scenario1, scenario2, scenario3, scenario4, scenario5, scenario6 };
            modelling.TransitionSteps = new Step[] { reporting };

            var ifrs9Root = new RunStep(ifrs9);
            var runSourceData = new RunStep(sourceData);
            var runCounterparties = new RunStep(counterparties);
            var runPd = new RunStep(pd);
            var runLgd = new RunStep(lgd);
            var runTransform = new RunStep(transform);
            var runApplyPd = new RunStep(applyPd);
            var runApplyLgd = new RunStep(applyLgd);

            var runModelling = new RunStep(modelling);
            var runScenario1 = new RunStep(scenario1);
            var runScenario2 = new RunStep(scenario2);
            var runScenario3 = new RunStep(scenario3);
            var runScenario4 = new RunStep(scenario4);
            var runScenario5 = new RunStep(scenario5);
            var runScenario6 = new RunStep(scenario6);

            var runReporting = new RunStep(reporting);

            ifrs9Root.TransitionSteps.Add(runSourceData);
            runSourceData.SubSteps.AddRange(new[] { runCounterparties, runPd, runLgd });
            runSourceData.TransitionSteps.Add(runTransform);
            runTransform.SubSteps.Add(runApplyPd);
            runApplyPd.SubSteps.Add(runApplyLgd);
            runTransform.TransitionSteps.Add(runModelling);
            runModelling.SubSteps.AddRange(new[] { runScenario1, runScenario2, runScenario3, runScenario4, runScenario5, runScenario6 });
            runModelling.TransitionSteps.Add(runReporting);
            return ifrs9Root;
        }

        private RunStep GetLetters(int runId, string runName)
        {
            var getLetters = new Step();
            getLetters.RunId = runId;
            getLetters.RunName = runName;
            getLetters.StepId = 1;
            getLetters.StepName = "Get Letters";
            getLetters.TargetAssembly = "TestLibrary.dll";
            getLetters.TargetType = "TestLibrary.TestRunner";
            getLetters.Payload = "1000|Hello";
            getLetters.Urls = new[] { "http://localhost:5000" };
            getLetters.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var distributeProcessing = new Step();
            distributeProcessing.RunId = runId;
            distributeProcessing.RunName = runName;
            distributeProcessing.StepId = 2;
            distributeProcessing.StepName = "Distribute Processing";

            var customers = new Step();
            customers.RunId = runId;
            customers.RunName = runName;
            customers.StepId = 21;
            customers.StepName = "Customers";
            customers.TargetAssembly = "TestLibrary.dll";
            customers.TargetType = "TestLibrary.TestRunner";
            customers.Payload = "3000|Sub Step 2.1";
            customers.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var nonCustomers = new Step();
            nonCustomers.RunId = runId;
            nonCustomers.RunName = runName;
            nonCustomers.StepId = 22;
            nonCustomers.StepName = "Non-Customers";
            nonCustomers.TargetAssembly = "TestLibrary.dll";
            nonCustomers.TargetType = "TestLibrary.TestRunner";
            nonCustomers.Payload = "5000|Sub Step 2.2";
            nonCustomers.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var responseRequired = new Step();
            responseRequired.RunId = runId;
            responseRequired.RunName = runName;
            responseRequired.StepId = 221;
            responseRequired.StepName = "Response Required";
            responseRequired.TargetAssembly = "TestLibrary.dll";
            responseRequired.TargetType = "TestLibrary.TestRunner";
            responseRequired.Payload = "5000|Sub Step 2.2.1";
            responseRequired.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"

            };

            var responseNotRequired = new Step();
            responseNotRequired.RunId = runId;
            responseNotRequired.RunName = runName;
            responseNotRequired.StepId = 222;
            responseNotRequired.StepName = "Response Not Required";
            responseNotRequired.TargetAssembly = "TestLibrary.dll";
            responseNotRequired.TargetType = "TestLibrary.TestRunner";
            responseNotRequired.Payload = "5000|Sub Step 2.2.2";
            responseNotRequired.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"

            };
            
            var offline = new Step();
            offline.RunId = runId;
            offline.RunName = runName;
            offline.StepId = 23;
            offline.StepName = "Offline";
            offline.TargetAssembly = "TestLibrary.dll";
            offline.TargetType = "TestLibrary.TestRunner";
            offline.Payload = "1000|Sub Step 2.3";
            offline.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var noLetterRequired = new Step();
            noLetterRequired.RunId = runId;
            noLetterRequired.RunName = runName;
            noLetterRequired.StepId = 24;
            noLetterRequired.StepName = "No Letter Required";
            noLetterRequired.TargetAssembly = "TestLibrary.dll";
            noLetterRequired.TargetType = "TestLibrary.TestRunner";
            noLetterRequired.Payload = "2000|Goodbye";
            noLetterRequired.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            var publish = new Step();
            publish.RunId = runId;
            publish.RunName = runName;
            publish.StepId = 3;
            publish.StepName = "Publish";
            publish.TargetAssembly = "TestLibrary.dll";
            publish.TargetType = "TestLibrary.TestRunner";
            publish.Payload = "1500|Goodbye";
            publish.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\DipRunner.dll",
                @"C:\GitHub\Binaries\Monitor\TestDependency.dll",
                @"C:\GitHub\Binaries\Monitor\TestLibrary.dll"
            };

            getLetters.TransitionSteps = new Step[] { distributeProcessing };
            distributeProcessing.SubSteps = new Step[] { customers, nonCustomers, offline, noLetterRequired };
            distributeProcessing.TransitionSteps = new Step[] { publish };
            nonCustomers.SubSteps = new Step[] { responseRequired, responseNotRequired };

            var runGetLetters = new RunStep(getLetters);
            var runDistributeProcessing = new RunStep(distributeProcessing);
            var runCustomers = new RunStep(customers);
            var runNonCustomers = new RunStep(nonCustomers);
            var runResponseRequired = new RunStep(responseRequired);
            var runResponseNotRequired = new RunStep(responseNotRequired);
            var runOffline = new RunStep(offline);
            var runNoLetterRequired = new RunStep(noLetterRequired);
            var runPublish = new RunStep(publish);

            runGetLetters.TransitionSteps.Add(runDistributeProcessing);

            runNonCustomers.SubSteps.AddRange(new[] { runResponseRequired, runResponseNotRequired });

            runDistributeProcessing.SubSteps.AddRange(new[] { runCustomers, runNonCustomers, runOffline, runNoLetterRequired });

            runDistributeProcessing.TransitionSteps.Add(runPublish);

            return runGetLetters;
        }
    }
}
