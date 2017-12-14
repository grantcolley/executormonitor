using DevelopmentInProgress.ExecutorMonitor.Wpf.Model;
using DipRunner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Services
{
    public interface IMonitorService
    {
        Task<List<Run>> GetRuns();
    }
}