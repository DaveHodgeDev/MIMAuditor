using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MCS.EventLogMonitor;
using System.IO;

namespace MCS.EventLogScavanger
{
    class Program
    {
        public EventLogMonitorConfig Config { get; set; }

        // Auditing tool's LogFile
        public static string LogFileName { get; set; }

        // Path to not uploaded JSON Files
        public static string jsonFileDirectory { get; set; }

        public static LogAnalyticsWorkspaceHelper API { get; set; }

        static void Main(string[] args)
        {
            EventLogMonitorConfig Config;
            Config = new EventLogMonitorConfig();

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
            Log logger = new Log(Config.LogFileName, Config.API)
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
                // Set a variable to the pending JSON uploads.
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
                        logger.Augment(strJSON, true);
                        logger.EventGeneratedTime = logger.EventGeneratedTime;
                        logger.WriteLog();

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

        static void Main_Test(string[] args)
        {
            EventLogMonitorConfig Config;
            Config = new EventLogMonitorConfig();

            //*************************************************************************************
            // EventLog - name of this applications event log
            //*************************************************************************************
            if (!EventLog.Exists(Config.EventLogName))
            {
                EventLog.CreateEventSource(Config.EventSource, Config.EventLogName);
            }

            string strJSON = File.ReadAllText(@"C:\Users\<userID>\Source\Workspaces\Azure\HybridReporting\EventLogExtractorConsole\MCS.EventLogScavanger\test.json");
            Log logger = new Log(Config.LogFileName, Config.API)
            {
                //TimeWritten = entry.TimeWritten,
                MachineName = Environment.MachineName,
                EntryType = "Information",
                Source = Config.EventLogName,
                EventId = 4121,
                User = ""
            };

            logger.Augment(strJSON);

            logger.EventGeneratedTime = logger.EventGeneratedTime;

            logger.WriteLog();
        }
    }
}

