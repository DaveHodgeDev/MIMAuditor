A Windows Service that monitors the EventLog for entries created by MIM.
It uploads the EventLog entries into the specified Log Analytics workspace.
If the Internet service is unavailable, the eventlog entries are written to the JSON File Directory folder listed in the .config file..
