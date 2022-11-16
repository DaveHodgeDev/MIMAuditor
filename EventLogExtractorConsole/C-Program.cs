using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EventLogExtractorConsole
{
    class Program
    {
        const string EventSource = "MCS.LogAnalytics.Collector";//MCS.HybridReportLogger
        const string EventLogName = "MCS Azure Monitor Workspace Collector";

        // Auditing tool's LogFile
        public static string LogFileName { get; set; }

        // Path to not uploaded JSON Files
        public static string jsonFileDirectory { get; set; }

        public static LogAnalyticsWorkspaceHelper API { get; set; }

        public static bool DebugMode { get; set; }


        private static Dictionary<Guid, string[]> partialEventDictionary;    //stores all partial requests organized by GUID

        public static string logName { get; set; }

        private static void Initialize()
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

            Console.WriteLine("DebugMode={0}", DebugMode);

            LogFileName = ConfigurationManager.AppSettings["logFile"];

            if (string.IsNullOrEmpty(LogFileName))
            {
                Console.WriteLine("No LogFile defined");
            }

            jsonFileDirectory = ConfigurationManager.AppSettings["jsonFileDirectory"];

            if (string.IsNullOrEmpty(jsonFileDirectory))
            {
                Console.WriteLine("No jsonFileDirectory defined");
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


        // *************************************************************************************
        // Enumerate JSON files in the folder. After processing successfully, delete the file
        // *************************************************************************************
        static void Main3() //_ProcessFromFile()
        {
            // Initialize Configuration Parameters
            Initialize();

            Log logger = new Log(LogFileName, API);

            try
            {
                // Set a variable to the pending JSON uploads.
                DirectoryInfo DirInfo = new DirectoryInfo(jsonFileDirectory);

                foreach (var fi in DirInfo.EnumerateFiles())
                {
                    try
                    {
                        Console.WriteLine("##############################################");
                        Console.WriteLine("File {0}", fi.FullName);
                        Console.WriteLine("##############################################");

                        string strJSON = File.ReadAllText(fi.FullName);
                        logger.Augment(strJSON);
                        logger.WriteLog();

                        //fi.Delete();
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
        }


        // *************************************************************************************
        // Process Single file for dev purposes only
        // *************************************************************************************
        static void main4()
        {
            Initialize();

            string strJSON = File.ReadAllText(@"D:\MIM\EventLogExtractorConsole\EventLogExtractorConsole\Approvals_MultipleExplicitMembers.json");
            Log logger = new Log(LogFileName, API);
            logger.Augment(strJSON);
            logger.WriteLog();
        }

        // *************************************************************************************
        // Main Program
        // *************************************************************************************

        static void Main(string[] args)
        {

            //#region read settings
            //var debug = ConfigurationManager.AppSettings["debug"];
            //    if (!string.IsNullOrEmpty(debug) && (debug.ToLower() == "true" || debug.ToLower() == "yes" || debug.ToLower() == "y")) DebugMode = true;
            //    else DebugMode = false;
            //    Console.WriteLine("DebugMode={0}", DebugMode);

            //    LogFileName = ConfigurationManager.AppSettings["logFile"];
            //    if (string.IsNullOrEmpty(LogFileName)) Console.WriteLine("No LogFile defined");

            //    jsonFileDirectory = ConfigurationManager.AppSettings["jsonFileDirectory"];
            //    if (string.IsNullOrEmpty(jsonFileDirectory)) Console.WriteLine("No jsonFileDirectory defined");

            //    var logName = ConfigurationManager.AppSettings["logName"];
            //    if (string.IsNullOrEmpty(logName)) throw new Exception("logName not set in app.config");

            //    var workspaceId = ConfigurationManager.AppSettings["workspaceId"];
            //    var workspaceKey = ConfigurationManager.AppSettings["workspaceKey"];
            //    var workspaceLogName = ConfigurationManager.AppSettings["workspaceLogName"];

            //    if (!string.IsNullOrEmpty(workspaceId) && !string.IsNullOrEmpty(workspaceKey) && !string.IsNullOrEmpty(workspaceLogName))
            //    {
            //        API = new LogAnalyticsWorkspaceHelper(workspaceId, workspaceKey, workspaceLogName);
            //    }
            //    #endregion

            Initialize();

            // *************************************************************************************
            // EventLog - name of this applications event log
            // *************************************************************************************
            if (!EventLog.Exists(EventLogName))
            {
                EventLog.CreateEventSource(EventSource, EventLogName);
            }

            // *************************************************************************************
            // Initialize Event Dictionary
            // *************************************************************************************
            partialEventDictionary = new Dictionary<Guid, string[]>();

            // *************************************************************************************
            // Set Raising Events on Identity Manager Request Log
            // *************************************************************************************
            EventLog log = new EventLog();
            log.Log = logName;
            log.EntryWritten += Log_EntryWritten;
            log.EnableRaisingEvents = true;

            // *************************************************************************************
            // Application started
            // *************************************************************************************
            EventLog.WriteEntry(EventSource, "Monitor Started", EventLogEntryType.Information, 101, 0);

            // *************************************************************************************
            // Run the application until a spacebar
            // *************************************************************************************
            ConsoleKeyInfo ketInfo;
            do
            {
                ketInfo = Console.ReadKey();
            }
            while (ketInfo.Key != ConsoleKey.Spacebar);

            // *************************************************************************************
            // Application exit
            // *************************************************************************************
            EventLog.WriteEntry(EventSource, "Monitor Stopped", EventLogEntryType.Information, 100, 0);
        }

        /// <summary>
        /// Log Entry Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Log_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            string[] partialRequestArray;  // array for storing all the partial requests
            Guid requestGuid = new Guid("00000000-0000-0000-0000-000000000000");

            EventLogEntry entry = e.Entry;
            //message moved into try
            #region construct log base structure
            Log log = new Log(LogFileName, API)
            {
                EventGeneratedTime = entry.TimeGenerated.ToUniversalTime(),
                //TimeWritten = entry.TimeWritten,
                MachineName = entry.MachineName,
                EntryType = entry.EntryType.ToString(),
                Source = entry.Source,
                EventId = entry.InstanceId,
                User = entry.UserName
            };
            #endregion

            try
            {
                string message;

                switch (e.Entry.InstanceId)
                {
                    case 4121:
                        message = entry.Message;
                        log.Augment(message);
                        log.WriteLog();
                        break;
                    case 4137:
                        message = entry.Message;
                        string pattern = string.Empty;


                        //if (DebugMode) pattern = "Request '([^']+)', message ([0-9]+) out of ([0-9]+): (.*)";   //pattern to detect unit-test
                        //else 

                        pattern = "Request '([^']+)', message ([0-9]+) out of ([0-9]+): \\n(.*)$";         //Pattern in Hybrid Reporting Module

                        Match match = (new Regex(pattern, RegexOptions.IgnoreCase)).Match(message);
                        if (match.Success)
                        {
                            string requestGuidString = string.Empty;
                            int startIndex;
                            int endIndex;
                            string matchedPartialMessageBody;

                            requestGuidString = match.Groups[1].Value;
                            if (string.IsNullOrEmpty(requestGuidString)) throw new Exception("partial request did not contain GUID");
                            startIndex = Convert.ToInt32(match.Groups[2].Value);
                            endIndex = Convert.ToInt32(match.Groups[3].Value);
                            matchedPartialMessageBody = match.Groups[4].Value;
                            requestGuid = new Guid(requestGuidString);

                            if (!partialEventDictionary.ContainsKey(requestGuid))
                            {
                                partialRequestArray = new string[endIndex];                     //create a new array of size endIndex
                                partialEventDictionary[requestGuid] = partialRequestArray;      //store new array in the dictionary
                            }
                            else
                            {
                                partialRequestArray = partialEventDictionary[requestGuid];      //gets the array from the dictionary
                            }
                            partialRequestArray[startIndex - 1] = matchedPartialMessageBody;    //stores the matched partial data in the array

                            if (!partialRequestArray.Contains<string>(null))                    //no more empty slots
                            {
                                partialEventDictionary.Remove(requestGuid);
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < partialRequestArray.Length; i++)
                                {
                                    sb.Append(partialRequestArray[i]);
                                }
                                message = sb.ToString();
                                log.Augment(message);
                                log.WriteLog();
                            }
                        }
                        else
                        {
                            EventLog.WriteEntry(EventSource, "No partial request detected for event 4137", EventLogEntryType.Warning, 300, 2);
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(EventSource, ex.Message, EventLogEntryType.Error, 200, 1);
            }
        }
    }
}
