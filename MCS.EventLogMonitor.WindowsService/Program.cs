using System.ServiceProcess;

namespace MCS.EventLogMonitor.WindowsService
{
    static class Program
    {
     /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                    new EventLogAgentService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
