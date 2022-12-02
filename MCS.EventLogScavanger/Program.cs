using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using MCS.EventLogMonitor;
using System.IO;
using Encrypt_Class;

namespace MCS.EventLogScavanger
{
    class Program
    {
        static void Main(string[] args)
        {
            ScavengerConfig Config = new ScavengerConfig();

            //*************************************************************************************
            // EventLog - name of this applications event log
            //*************************************************************************************
            if (!EventLog.Exists(Config.EventLogName))
            {
                EventLog.CreateEventSource(Config.EventSource, Config.EventLogName);
            }

            //*************************************************************************************
            // Application started
            //*************************************************************************************
            EventLog.WriteEntry(Config.EventSource, "Scavenger Started", EventLogEntryType.Information, 2000, 0);

            //*************************************************************************************
            // Enumerate JSON files in the folder. After processing successfully, delete the file
            //*************************************************************************************
            Log log = new Log(Config.LogFileName, Config.API)
            {
                //TimeWritten = entry.TimeWritten,
                MachineName = Environment.MachineName,
                EntryType = "Information",
                Source = Config.EventLogName,
                EventId = 4121,
                User = ""
            };

            try
            {
                // Gather the JSON files to upload from the JSON file directory.
                DirectoryInfo DirInfo = new DirectoryInfo(Config.jsonFileDirectory);

                int i = 0;
                int files = DirInfo.EnumerateFiles().Count();
                foreach (var fi in DirInfo.EnumerateFiles())
                {
                    i++;

                    try
                    {
                        Console.WriteLine("##############################################");
                        Console.WriteLine("File ({0} of {1}): {2} ", i, files, fi.FullName);
                        Console.WriteLine("##############################################");

                        string strJSON = File.ReadAllText(fi.FullName);

                        log.Augment(strJSON, false);

                        log.WriteLog();

                        fi.Delete();
                    }
                    catch (UnauthorizedAccessException unAuthTop)
                    {
                        Console.WriteLine($"{unAuthTop.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }

            //*************************************************************************************
            // Application exit
            //*************************************************************************************
            EventLog.WriteEntry(Config.EventSource, "Scavenger Stopped", EventLogEntryType.Information, 2001, 0);
        }
    }

    public class ScavengerConfig
    {
        public LogAnalyticsWorkspaceHelper API { get; set; }

        public string certificate { get; set; }

        public static bool DebugMode { get; set; }

        public string EventLogName = "MCS Azure Monitor Workspace Collector";

        public string EventSource = "MCS.LogAnalytics.Scavenger";

        // Path to place JSON Files due to a parsing/upload issue
        public string jsonFileDirectory { get; set; }

        /// <summary>
        /// MIM Auditor's LogFile
        /// </summary>
        public string LogFileName { get; set; }

        public string logName { get; set; }

        public ScavengerConfig()
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

            string workspaceId = ConfigurationManager.AppSettings["workspaceId"];
            string workspaceKey = ConfigurationManager.AppSettings["workspaceKey"];
            string workspaceLogName = ConfigurationManager.AppSettings["workspaceLogName"];

            workspaceKey = myEncrypt.decryptRsa(workspaceKey);

            if (!string.IsNullOrEmpty(workspaceId) && !string.IsNullOrEmpty(workspaceKey) && !string.IsNullOrEmpty(workspaceLogName))
            {
                API = new LogAnalyticsWorkspaceHelper(workspaceId, workspaceKey, workspaceLogName);
            }
            #endregion
        }
    }
}

