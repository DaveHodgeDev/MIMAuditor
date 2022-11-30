using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Encrypt_Class;


namespace MCS.EventLogMonitor
{
    public class EventLogMonitorConfig
    {
        public LogAnalyticsWorkspaceHelper API { get; set; }
        
        public string certificate { get; set; }

        public static bool DebugMode { get; set; }

        public string EventLogName = "MCS Azure Monitor Workspace Collector";

        public string EventSource = "MCS.LogAnalytics.Collector";

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

            certificate = ConfigurationManager.AppSettings["certificate"];

            if (string.IsNullOrEmpty(certificate))
            {
                //throw("No certificate defined");
            }

            LogFileName = ConfigurationManager.AppSettings["logFile"];

            if (string.IsNullOrEmpty(LogFileName))
            {
                //Console.WriteLine("No LogFile defined");
            }

            logName = ConfigurationManager.AppSettings["logName"];

            if (string.IsNullOrEmpty(logName))
            {
                throw new Exception("logName not set in app.config");
            }

            jsonFileDirectory = ConfigurationManager.AppSettings["jsonFileDirectory"];

            if (string.IsNullOrEmpty(jsonFileDirectory))
            {
                //Console.WriteLine("No jsonFileDirectory defined");
            }

            // Calling the decryption code...
            Encrypt myEncrypt = new Encrypt();

            // Get the certificate 
            myEncrypt.certificateName = certificate;

            myEncrypt.getCertificate(myEncrypt.certificateName);
            
            // Decryption 
            string sDecryptedSecret = string.Empty;

            var workspaceId = ConfigurationManager.AppSettings["workspaceId"];
            var workspaceKey = ConfigurationManager.AppSettings["workspaceKey"];
            var workspaceLogName = ConfigurationManager.AppSettings["workspaceLogName"];

            workspaceKey = myEncrypt.decryptRsa(workspaceKey);

            if (!string.IsNullOrEmpty(workspaceId) && !string.IsNullOrEmpty(workspaceKey) && !string.IsNullOrEmpty(workspaceLogName))
            {
                API = new LogAnalyticsWorkspaceHelper(workspaceId, workspaceKey, workspaceLogName);
            }
            #endregion
        }
    }
}
