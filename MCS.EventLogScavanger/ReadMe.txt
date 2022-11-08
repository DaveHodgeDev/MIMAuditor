Watches a folder for EventLog Extract. If found, upload to specified Log Analytics Workspace.

This tool works in conjunction with the MCS.EventLogMonitor.WindowsService as a backup service to cleanup EventLog dump created when Internet service is not available.

This tool is meant to be run with Windows Scheduler. Schedule to run no more frequent than once every 12 hours.

Test