using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace MCS.EventLogMonitor
{
    public class EventLogMonitorConfig
    {
        public LogAnalyticsWorkspaceHelper API { get; set; }

        public string EventSource = "MCS.LogAnalytics.Collector";

        public string EventLogName = "MCS Azure Monitor Workspace Collector";

        public static bool DebugMode { get; set; }

        //private static Dictionary<Guid, string[]> partialEventDictionary;    //stores all partial requests organized by GUID

        // Path to place JSON Files due to a parsing/upload issue
        public string jsonFileDirectory { get; set; }

        /// <summary>
        /// MIM Auditor's LogFile
        /// </summary>
        public string LogFileName { get; set; }

        public string logName { get; set; }

        public EventLogMonitorConfig()
        {
            #region read settings

            var debug = ConfigurationManager.AppSettings["debug"];

            if (!string.IsNullOrEmpty(debug) && (debug.ToLower() == "true" || debug.ToLower() == "yes" || debug.ToLower() == "y"))
            {
                DebugMode = true;
            }
            else
            {
                DebugMode = false;
            }

            //Console.WriteLine("DebugMode={0}", DebugMode);

            LogFileName = ConfigurationManager.AppSettings["logFile"];

            if (string.IsNullOrEmpty(LogFileName))
            {
                //Console.WriteLine("No LogFile defined");
            }

            jsonFileDirectory = ConfigurationManager.AppSettings["jsonFileDirectory"];

            if (string.IsNullOrEmpty(jsonFileDirectory))
            {
                //Console.WriteLine("No jsonFileDirectory defined");
            }

            logName = ConfigurationManager.AppSettings["logName"];

            if (string.IsNullOrEmpty(logName))
            {
                throw new Exception("logName not set in app.config");
            }

            var workspaceId = ConfigurationManager.AppSettings["workspaceId"];
            var workspaceKey = ConfigurationManager.AppSettings["workspaceKey"];
            var workspaceLogName = ConfigurationManager.AppSettings["workspaceLogName"];

            if (!string.IsNullOrEmpty(workspaceId) && !string.IsNullOrEmpty(workspaceKey) && !string.IsNullOrEmpty(workspaceLogName))
            {
                API = new LogAnalyticsWorkspaceHelper(workspaceId, workspaceKey, workspaceLogName);
            }
            #endregion
        }
    }
}
