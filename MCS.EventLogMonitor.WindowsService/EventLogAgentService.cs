using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
//using MCS.EventLogMonitor;

namespace MCS.EventLogMonitor.WindowsService
{
    public partial class EventLogAgentService : ServiceBase
    {
        private static Dictionary<Guid, string[]> partialEventDictionary;    //stores all partial requests organized by GUID

        public static EventLogMonitorConfig Config { get; set; }

        public EventLogAgentService()
        {
            InitializeComponent();
            Config = new EventLogMonitorConfig();
        }

        protected override void OnStart(string[] args)
        {
            //*************************************************************************************
            // EventLog - name of this applications event log
            //*************************************************************************************
            if (!EventLog.Exists(Config.EventLogName))
            {
                EventLog.CreateEventSource(Config.EventSource, Config.EventLogName);
            }
            
            //*************************************************************************************
            // Initialize Event Dictionary
            //*************************************************************************************
            partialEventDictionary = new Dictionary<Guid, string[]>();

            //*************************************************************************************
            // Set Raising Events on Identity Manager Request Log
            //*************************************************************************************
            //EventLog log = new EventLog();
            //log.Log = Config.logName;
            //log.EntryWritten += Log_EntryWritten;
            //log.EnableRaisingEvents = true;

            EventLogQuery subscriptionQuery = new EventLogQuery("Identity Manager Request Log", PathType.LogName);
            EventLogWatcher watcher = new EventLogWatcher(subscriptionQuery);
            watcher.EventRecordWritten += Watcher_EventRecordWritten;
            watcher.Enabled = true;

            //*************************************************************************************
            // Application started
            //*************************************************************************************
            EventLog.WriteEntry(Config.EventSource, "Monitor Started", EventLogEntryType.Information, 100, 0);
        }

        protected override void OnStop()
        {
            //*************************************************************************************
            // Application exit
            //*************************************************************************************
            //watcher.Enabled = false;
            EventLog.WriteEntry(Config.EventSource, "Monitor Stopped", EventLogEntryType.Information, 101, 0);
        }

        private static void Watcher_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            string[] partialRequestArray;  // array for storing all the partial requests
            Guid requestGuid = new Guid("00000000-0000-0000-0000-000000000000");

            EventRecord entry = e.EventRecord;

            string message = entry.FormatDescription();

            //#region construct log base structure
            
            Log log = new Log(Config.LogFileName, Config.API)
            {
                EventGeneratedTime = DateTime.Parse(entry.TimeCreated.ToString()),
                MachineName = entry.MachineName,
                EntryType = entry.LevelDisplayName.ToString(),
                Source = entry.ProviderName.ToString(),
                EventId = entry.Id,
                // User = entry.UserName
            };
                        
            //#endregion

            try
            {
                switch (entry.Id)
                {
                    case 4121:
                        log.Augment(message,false);
                        log.WriteLog();
                        break;
                    case 4137:
                        string pattern = string.Empty;
                        //if (Config.DebugMode) pattern = "Request '([^']+)', message ([0-9]+) out of ([0-9]+): (.*)";   //pattern to detect unit-test
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
                                log.Augment(message,false);
                                log.WriteLog();
                            }
                        }
                        else
                        {
                            // EventLog.WriteEntry(Config.EventSource, "No partial request detected for event 4137", EventLogEntryType.Warning, 300, 2);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Guid g = Guid.NewGuid();
                string filename = Config.jsonFileDirectory + "\\PreParse-" + g.ToString() + ".json";
                System.IO.File.WriteAllText(filename, message);

                EventLog.WriteEntry(Config.EventSource, "Pre-Parse (Pre-Parse - " + g.ToString() + ".json) - " + ex, System.Diagnostics.EventLogEntryType.Information, 200);
            }
        }



        /// <summary>
        /// Log Entry Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Log_EntryWritten(object sender, EntryWrittenEventArgs e)
        //{
        //    string[] partialRequestArray;  // array for storing all the partial requests
        //    Guid requestGuid = new Guid("00000000-0000-0000-0000-000000000000");

        //    EventLogEntry entry = e.Entry;
        //    string message = entry.Message;

        //    #region construct log base structure
        //    Log log = new Log(Config.LogFileName, Config.API)
        //    {
        //        EventGeneratedTime = entry.TimeGenerated,
        //        //TimeWritten = entry.TimeWritten,
        //        MachineName = entry.MachineName,
        //        EntryType = entry.EntryType.ToString(),
        //        Source = entry.Source,
        //        EventId = entry.InstanceId,
        //        User = entry.UserName
        //    };
        //    #endregion

        //    try
        //    {
        //        switch (e.Entry.InstanceId)
        //        {
        //            case 4121:
        //                log.Augment(message);
        //                log.WriteLog();
        //                break;
        //            case 4137:
        //                string pattern = string.Empty;
        //                //if (Config.DebugMode) pattern = "Request '([^']+)', message ([0-9]+) out of ([0-9]+): (.*)";   //pattern to detect unit-test
        //                //else
        //                pattern = "Request '([^']+)', message ([0-9]+) out of ([0-9]+): \\n(.*)$";         //Pattern in Hybrid Reporting Module

        //                Match match = (new Regex(pattern, RegexOptions.IgnoreCase)).Match(message);
        //                if (match.Success)
        //                {
        //                    string requestGuidString = string.Empty;
        //                    int startIndex;
        //                    int endIndex;
        //                    string matchedPartialMessageBody;

        //                    requestGuidString = match.Groups[1].Value;
        //                    if (string.IsNullOrEmpty(requestGuidString)) throw new Exception("partial request did not contain GUID");
        //                    startIndex = Convert.ToInt32(match.Groups[2].Value);
        //                    endIndex = Convert.ToInt32(match.Groups[3].Value);
        //                    matchedPartialMessageBody = match.Groups[4].Value;
        //                    requestGuid = new Guid(requestGuidString);

        //                    if (!partialEventDictionary.ContainsKey(requestGuid))
        //                    {
        //                        partialRequestArray = new string[endIndex];                     //create a new array of size endIndex
        //                        partialEventDictionary[requestGuid] = partialRequestArray;      //store new array in the dictionary
        //                    }
        //                    else
        //                    {
        //                        partialRequestArray = partialEventDictionary[requestGuid];      //gets the array from the dictionary
        //                    }
        //                    partialRequestArray[startIndex - 1] = matchedPartialMessageBody;    //stores the matched partial data in the array

        //                    if (!partialRequestArray.Contains<string>(null))                    //no more empty slots
        //                    {
        //                        partialEventDictionary.Remove(requestGuid);
        //                        StringBuilder sb = new StringBuilder();
        //                        for (int i = 0; i < partialRequestArray.Length; i++)
        //                        {
        //                            sb.Append(partialRequestArray[i]);
        //                        }
        //                        message = sb.ToString();
        //                        log.Augment(message);
        //                        log.WriteLog();
        //                    }
        //                }
        //                else
        //                {
        //                    EventLog.WriteEntry(Config.EventSource, "No partial request detected for event 4137", EventLogEntryType.Warning, 300, 2);
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MCS.EventLogMonitor.EventLogMonitorConfig config = new EventLogMonitorConfig();

        //        //Full JSON
        //        Guid g = Guid.NewGuid();
        //        string filename = config.jsonFileDirectory + "\\PreParse-" + g.ToString() + ".json";
        //        System.IO.File.WriteAllText(filename, message);

        //        EventLog.WriteEntry(Config.EventSource, "Pre-Parse (Pre-Parse - " + g.ToString() + ".json) - " + ex, System.Diagnostics.EventLogEntryType.Information, 200);
        //        //EventLog.WriteEntry(Config.EventSource, ex.Message, EventLogEntryType.Error, 200, 1);
        //    }
        //}

    }
}
