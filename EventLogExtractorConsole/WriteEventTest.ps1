Function CreateEntry
{
	Param([int] $EventID, [string] $Message)
	Write-EventLog -LogName "Identity Manager Request Log" -Source “Microsoft.IdentityManagement.Service” -EntryType Information -EventID $EventID -Message $message
}

###############################################################################
# Create the EventLog to store the MIM Request Events in.
###############################################################################
If ([System.Diagnostics.EventLog]::Exists('Identity Manager Request Log') -ne $true)
{
    New-EventLog -LogName "Identity Manager Request Log" -Source "Microsoft.IdentityManagement.Service"
}
Else
{
    Write-Host "Event log already exists!" -ForegroundColor Green
}

# Test Record to add new user
# Test Record to add new group
# Test Record to add new mcsContact

# Test Record to add 2 users to Administrators set
$log = '{"Calculated":"false","Propertyname":"ExplicitMember","Value":"[{\"HybridObjectID\":\"b03f04fb-a830-4d41-afc7-fdfd4b65c0d1\",\"AccountName\":\"chodge\",\"DepartmentNumber\":null,\"DisplayName\":\"Christina Hodge\",\"Domain\":\"DAVRISLABS\"}]","Operation":"Create","Mode":"Add"},{"Calculated":"false","Propertyname":"ExplicitMember","Value":"[{\"HybridObjectID\":\"fe9b6e79-2144-4cfa-a59f-111f4e3caecc\",\"AccountName\":\"dhodge4\",\"DepartmentNumber\":null,\"DisplayName\":\"Daniel Hodge\",\"Domain\":\"DAVRISLABS\"}]","Operation":"Create","Mode":"Add"}'
CreateLogEntry 4121 $log

#Delete when CreateLogEntry is validated
#Write-EventLog -LogName "Identity Manager Request Log" -Source “Microsoft.IdentityManagement.Service” -EntryType Information -EventID 4121 -Message $log

# Multipart Message
Write-EventLog -LogName "Identity Manager Request Log" -Source “Microsoft.IdentityManagement.Service” -EntryType Information -EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 1 out of 3: {""HybridObjectID"":""2643ad6b-3a59-4157-b2b6-bb4131470881"",""ObjectType"":""Request"","
Write-EventLog -LogName "Identity Manager Request Log" -Source “Microsoft.IdentityManagement.Service” -EntryType Information -EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 2 out of 3: ""Creator"": {""HybridObjectID"": ""7fb2b853-24f0-4498-9534-4e10589723c4"",""CreatedTime"": ""2019-12-17 01:19:42.633"",""ObjectID"": ""2340"",""Creator"": ""2340"",""DomainConfiguration"": ""2730"",""AccountName"": ""miminstall"",""DisplayName"": ""miminstall"",""Domain"": ""DAVRISLABS"",""Email"": """",""MailNickname"": ""miminstall"",""ObjectType"": ""Person""},"
Write-EventLog -LogName "Identity Manager Request Log" -Source “Microsoft.IdentityManagement.Service” -EntryType Information -EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 3 out of 3: ""Operation"": ""Put"",  ""Target"": {""HybridObjectID"": ""7a3e5594-48dc-4aa5-b9bb-042153b17c5e"",""Creator"": ""2731"",""ObjectID"": ""4777"",""Manager"": ""84282"",""AccountName"": ""dhodge100"",},""RequestStatus"": ""Completed"",""DisplayName"": ""Update to Person:  'David Hodge' Request"",""CreatedTime"": ""2/13/2020 10:23:08 PM"",""TargetObjectType"": ""Person"",""CommittedTime"": ""2/13/2020 10:23:09 PM""}"