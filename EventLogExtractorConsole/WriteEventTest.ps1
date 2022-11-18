﻿########################################################################################################
# Purpose: Generate fictional test data in Identity Manager Request Log to evaluate Auditor service is 
# correctly processing the data and uploading to Log Analytics
########################################################################################################
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

# Replace DAVRISLABS with CONTOSO
# Replace DHODGE-ADMIN with hodge-admin
# Test Record to add new user
$log = '{"HybridObjectID":"86b46f1a-f912-4ad5-a58f-c31f1b38709a","CommittedTime":"2022-11-16 16:25:17.680","CreatedTime":"2022-11-16 16:25:12.307","ExpirationTime":"2022-12-16 16:25:30.617","msidmCompletedTime":"2022-11-16 16:25:30.617","Creator":{"HybridObjectID":"7fb2b853-24f0-4498-9534-4e10589723c4","CreatedTime":"2019-11-26 20:26:08.653","ObjectID":"2340","Creator":"2340","DomainConfiguration":"2730","AccountName":"dhodge-admin","DisplayName":"dhodge-admin","Domain":"DAVRISLABS","Email":"","MailNickname":"dhodge-admin","ObjectType":"Person"},"ManagementPolicy":[{"HybridObjectID":"1e17ca38-e22c-4f8b-a8c2-2f8f86439c1f","Disabled":"0","GrantRight":"1","CreatedTime":"2019-11-26 20:26:08.653","PrincipalSet":"2732","ResourceCurrentSet":"2839","ResourceFinalSet":"2839","ObjectID":"2860","ActionParameter":["Description","DisplayName","ExpirationTime","AccountName","AD_UserCannotChangePassword","Address","Assistant","AuthNWFRegistered","City","AuthNWFLockedOut","AuthNLockoutRegistrationID","Company","CostCenter","CostCenterName","Country","Department","Domain","DomainConfiguration","Email","EmployeeEndDate","EmployeeID","EmployeeStartDate","EmployeeType","FirstName","FreezeCount","FreezeLevel","IsRASEnabled","JobTitle","LastName","LastResetAttemptTime","LoginName","MailNickname","Manager","MiddleName","MobilePhone","ObjectType","ObjectSID","OfficeFax","OfficeLocation","OfficePhone","Register","ResetPassword","Photo","PostalCode","ProxyAddressCollection","RegistrationRequired","TimeZone","msidmMFAPINCode","msidmPamLinkedUser","msidmPhoneGatePhoneNumber","mcsEmployeeStatus"],"ActionType":["Create","Add","Modify","Remove"],"Description":"Administration: Administrators can read and update Users","DisplayName":"Administration: Administrators can read and update Users","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"bf9d22d3-7899-4412-9564-97835d48aadf","Disabled":"0","GrantRight":"0","CreatedTime":"2020-08-18 21:10:43.247","Creator":"2340","PrincipalSet":"2733","ResourceCurrentSet":"2839","ResourceFinalSet":"2839","AuthorizationWorkflowDefinition":"4342","ObjectID":"4344","ActionParameter":"*","ActionType":["Create","Modify"],"Description":"Trigger Authorization workflows to validate user creation and modification requests.","DisplayName":"~mcs: Users: Validation: Verify Request on user creation and update","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"065a6c14-cb72-4a1e-b758-a082a0f065ae","Disabled":"0","GrantRight":"0","CreatedTime":"2020-08-18 21:10:48.137","Creator":"2340","PrincipalSet":"2733","ResourceFinalSet":"2839","ActionWorkflowDefinition":["4294","4323"],"ObjectID":"4367","ActionParameter":"*","ActionType":"Create","Description":"Trigger the workflows required during user creation.","DisplayName":"~mcs: Users: Event: Create user workflows","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"ddeb89d0-eb7f-4634-941e-f85ff48c35c4","Disabled":"0","GrantRight":"1","CreatedTime":"2020-08-18 21:12:20.600","Creator":"2340","PrincipalSet":"2732","ResourceCurrentSet":"2841","ResourceFinalSet":"2841","ObjectID":"4657","ActionParameter":"*","ActionType":["Add","Create","Delete","Modify","Read","Remove"],"DisplayName":"~mcs: Rights: Administrators can perform any action","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"}],"Target":{"HybridObjectID":"b35f8d79-05e9-4491-a37b-ecccf85792ed","AD_UserCannotChangePassword":"0","IsRASEnabled":"0","Register":"0","RegistrationRequired":"0","mcsResetPassword":"0","mcsPwdNeverExpires":"0","mcsProvisionADUser":"1","mcsSyncOn":"1","mcsUnlockAccount":"0","mcsExtendExpiration":"0","mcsDisableADAccount":"0","mcsEnableMailbox":"0","mcsExcludeFromTransfer":"0","mcsExistsInAD":"0","mcsmsExchRequireAuthToSendTo":"0","mcsMailboxEnabled":"0","mcsmsExchHideFromAddressLists":"0","CreatedTime":"2022-11-16 16:25:17.680","EmployeeStartDate":"2022-11-16 16:25:19.873","mcsuserAccountControl":"512","Creator":"2340","DomainConfiguration":"2730","Manager":"6589","ObjectID":"37401","AccountName":"bvilla2","Department":"Home Repair","DisplayName":"Villa, Bob","Domain":"DAVRISLABS","EmployeeType":"Employee","FirstName":"Bob","JobTitle":"Supervisor","LastName":"Villa","ObjectType":"Person","mcsADDN":"CN=bvilla2,OU=Users,OU=Global,DC=davrislabs,DC=com","mcsUserPrincipalName":"bvilla2@davrislabs.com","mcsEmployeeStatus":"Active"},"AuthorizationWorkflowInstance":{"HybridObjectID":"9fe441a6-3167-4826-8707-5958025ae1cf","CreatedTime":"2022-11-16 16:25:12.603","Creator":"2340","Requestor":"2340","WorkflowDefinition":"4342","Target":"37401","Request":"37402","ObjectID":"37403","DisplayName":"~mcs: Users: Validate create and modify request","ObjectType":"WorkflowInstance","WorkflowStatus":"Completed"},"ActionWorkflowInstance":[{"HybridObjectID":"9796f6a7-4906-44be-a3db-25d6dca9e058","CreatedTime":"2022-11-16 16:25:17.680","Creator":"2340","Requestor":"2340","WorkflowDefinition":"4294","Target":"37401","Request":"37402","ObjectID":"37404","DisplayName":"~mcs: Users: Generate unique attributes","ObjectType":"WorkflowInstance","WorkflowStatus":"Completed"},{"HybridObjectID":"cc3889a6-f166-414b-874a-e8b35f02acc8","CreatedTime":"2022-11-16 16:25:17.680","Creator":"2340","Requestor":"2340","WorkflowDefinition":"4323","Target":"37401","Request":"37402","ObjectID":"37405","DisplayName":"~mcs: Users: Set initial values for new portal users","ObjectType":"WorkflowInstance","WorkflowStatus":"Completed"}],"DisplayName":"Create Person:  'Bob Villa' Request","ObjectType":"Request","Operation":"Create","RequestStatus":"Completed","TargetObjectType":"Person","ServicePartitionName":"IDWEBADMIN.DAVRISLABS.COM","RequestParameter":[{"Calculated":"false","PropertyName":"AccountName","Value":"bvilla3","Operation":"Create"},{"Calculated":"false","PropertyName":"Department","Value":"Home Repair","Operation":"Create"},{"Calculated":"false","PropertyName":"DisplayName","Value":"Bob Villa","Operation":"Create"},{"Calculated":"false","PropertyName":"Domain","Value":"DAVRISLABS","Operation":"Create"},{"Calculated":"false","PropertyName":"EmployeeType","Value":"Employee","Operation":"Create"},{"Calculated":"false","PropertyName":"FirstName","Value":"Bob","Operation":"Create"},{"Calculated":"false","PropertyName":"JobTitle","Value":"Supervisor","Operation":"Create"},{"Calculated":"false","PropertyName":"LastName","Value":"Villa","Operation":"Create"},{"Calculated":"false","PropertyName":"Manager","Value":{"HybridObjectID":"eaeb30da-227e-4b07-a977-06fda5c6213f","AD_UserCannotChangePassword":"0","IsRASEnabled":"0","Register":"0","RegistrationRequired":"0","mcsResetPassword":"0","mcsPwdNeverExpires":"0","mcsProvisionADUser":"1","mcsSyncOn":"1","mcsUnlockAccount":"0","mcsExtendExpiration":"0","mcsDisableADAccount":"0","mcsEnableMailbox":"0","mcsExcludeFromTransfer":"0","mcsExistsInAD":"0","mcsmsExchRequireAuthToSendTo":"0","mcsmsExchHideFromAddressLists":"0","CreatedTime":"2022-02-18 13:39:00.697","EmployeeStartDate":"2022-02-18 13:39:02.570","mcsADCreationDate":"2022-02-18 13:47:58.000","mcsADPasswordLastSetDate":"2022-02-18 18:55:10.000","mcsuserAccountControl":"512","DomainConfiguration":"2730","Creator":"2731","ObjectID":"6589","Manager":"6602","AccountName":"dhodge2","Address":"1 MIM Way","City":"Barberton","Department":"Information Technology","DisplayName":"Dave Hodge","Domain":"DAVRISLABS","EmployeeID":"1000","EmployeeType":"Employee","FirstName":"Dave","JobTitle":"CSO","LastName":"Hodge","MiddleName":"A.","MVObjectID":"{C40761C8-3490-EC11-8B01-000D3A1B035A}","ObjectType":"Person","PostalCode":"44203","mcsADDN":"CN=dhodge2,OU=Users,OU=Global,DC=davrislabs,DC=com","mcsState":"OH","mcsUserPrincipalName":"dhodge2@davrislabs.com","mcsExtensionAttribute1":"Managed","mcsEmployeeStatus":"Active"},"Operation":"Create"},{"Calculated":"false","PropertyName":"ObjectType","Value":"Person","Operation":"Create"},{"Calculated":"false","PropertyName":"mcsEmployeeStatus","Value":"Active","Operation":"Create"},{"Calculated":"true","PropertyName":"ObjectID","Value":{"HybridObjectID":"b35f8d79-05e9-4491-a37b-ecccf85792ed","AD_UserCannotChangePassword":"0","IsRASEnabled":"0","Register":"0","RegistrationRequired":"0","mcsResetPassword":"0","mcsPwdNeverExpires":"0","mcsProvisionADUser":"1","mcsSyncOn":"1","mcsUnlockAccount":"0","mcsExtendExpiration":"0","mcsDisableADAccount":"0","mcsEnableMailbox":"0","mcsExcludeFromTransfer":"0","mcsExistsInAD":"0","mcsmsExchRequireAuthToSendTo":"0","mcsMailboxEnabled":"0","mcsmsExchHideFromAddressLists":"0","CreatedTime":"2022-11-16 16:25:17.680","EmployeeStartDate":"2022-11-16 16:25:19.873","mcsuserAccountControl":"512","Creator":"2340","DomainConfiguration":"2730","Manager":"6589","ObjectID":"37401","AccountName":"bvilla2","Department":"Home Repair","DisplayName":"Villa, Bob","Domain":"DAVRISLABS","EmployeeType":"Employee","FirstName":"Bob","JobTitle":"Supervisor","LastName":"Villa","ObjectType":"Person","mcsADDN":"CN=bvilla2,OU=Users,OU=Global,DC=davrislabs,DC=com","mcsUserPrincipalName":"bvilla2@davrislabs.com","mcsEmployeeStatus":"Active"},"Operation":"Create"},{"Calculated":"true","PropertyName":"Creator","Value":{"HybridObjectID":"7fb2b853-24f0-4498-9534-4e10589723c4","CreatedTime":"2019-11-26 20:26:08.653","ObjectID":"2340","Creator":"2340","DomainConfiguration":"2730","AccountName":"dhodge-admin","DisplayName":"dhodge-admin","Domain":"DAVRISLABS","Email":"","MailNickname":"dhodge-admin","ObjectType":"Person"},"Operation":"Create"},{"Calculated":"true","PropertyName":"DomainConfiguration","Value":{"HybridObjectID":"1aff46f4-5511-452d-bcbd-7f7b34b0fe14","IsConfigurationType":"0","CreatedTime":"2019-11-26 20:26:08.653","ObjectID":"2730","DisplayName":"DAVRISLABS","Domain":"DAVRISLABS","ObjectType":"DomainConfiguration"},"Operation":"Create"}]}'
CreateLogEntry 4121 $log

# Test Record to modify user
$log = '{"HybridObjectID":"ba9d66e0-0f2a-42f4-a492-22f45f039b09","CommittedTime":"2022-11-16 16:50:44.233","CreatedTime":"2022-11-16 16:50:43.353","ExpirationTime":"2022-12-16 16:50:52.387","msidmCompletedTime":"2022-11-16 16:50:52.370","Creator":{"HybridObjectID":"7fb2b853-24f0-4498-9534-4e10589723c4","CreatedTime":"2019-11-26 20:26:08.653","ObjectID":"2340","Creator":"2340","DomainConfiguration":"2730","AccountName":"dhodge-admin","DisplayName":"dhodge-admin","Domain":"DAVRISLABS","Email":"","MailNickname":"dhodge-admin","ObjectType":"Person"},"ManagementPolicy":[{"HybridObjectID":"1e17ca38-e22c-4f8b-a8c2-2f8f86439c1f","Disabled":"0","GrantRight":"1","CreatedTime":"2019-11-26 20:26:08.653","PrincipalSet":"2732","ResourceCurrentSet":"2839","ResourceFinalSet":"2839","ObjectID":"2860","ActionParameter":["Description","DisplayName","ExpirationTime","AccountName","AD_UserCannotChangePassword","Address","Assistant","AuthNWFRegistered","City","AuthNWFLockedOut","AuthNLockoutRegistrationID","Company","CostCenter","CostCenterName","Country","Department","Domain","DomainConfiguration","Email","EmployeeEndDate","EmployeeID","EmployeeStartDate","EmployeeType","FirstName","FreezeCount","FreezeLevel","IsRASEnabled","JobTitle","LastName","LastResetAttemptTime","LoginName","MailNickname","Manager","MiddleName","MobilePhone","ObjectType","ObjectSID","OfficeFax","OfficeLocation","OfficePhone","Register","ResetPassword","Photo","PostalCode","ProxyAddressCollection","RegistrationRequired","TimeZone","msidmMFAPINCode","msidmPamLinkedUser","msidmPhoneGatePhoneNumber","mcsEmployeeStatus"],"ActionType":["Create","Add","Modify","Remove"],"Description":"Administration: Administrators can read and update Users","DisplayName":"Administration: Administrators can read and update Users","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"3a1b5443-e9a3-4503-9c90-6981340b054e","Disabled":"0","GrantRight":"0","CreatedTime":"2020-08-18 21:10:29.497","Creator":"2340","PrincipalSet":"2733","ResourceCurrentSet":"4291","ResourceFinalSet":"4291","ActionWorkflowDefinition":"4294","ObjectID":"4296","ActionParameter":["AccountName","Company","FirstName","LastName","mcsDepartment","MiddleName"],"ActionType":"Modify","Description":"Trigger workflows on user update","DisplayName":"~mcs: Users: Event: Update user workflows","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"bf9d22d3-7899-4412-9564-97835d48aadf","Disabled":"0","GrantRight":"0","CreatedTime":"2020-08-18 21:10:43.247","Creator":"2340","PrincipalSet":"2733","ResourceCurrentSet":"2839","ResourceFinalSet":"2839","AuthorizationWorkflowDefinition":"4342","ObjectID":"4344","ActionParameter":"*","ActionType":["Create","Modify"],"Description":"Trigger Authorization workflows to validate user creation and modification requests.","DisplayName":"~mcs: Users: Validation: Verify Request on user creation and update","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"ddeb89d0-eb7f-4634-941e-f85ff48c35c4","Disabled":"0","GrantRight":"1","CreatedTime":"2020-08-18 21:12:20.600","Creator":"2340","PrincipalSet":"2732","ResourceCurrentSet":"2841","ResourceFinalSet":"2841","ObjectID":"4657","ActionParameter":"*","ActionType":["Add","Create","Delete","Modify","Read","Remove"],"DisplayName":"~mcs: Rights: Administrators can perform any action","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"}],"Target":{"HybridObjectID":"b35f8d79-05e9-4491-a37b-ecccf85792ed","AD_UserCannotChangePassword":"0","IsRASEnabled":"0","Register":"0","RegistrationRequired":"0","mcsResetPassword":"0","mcsPwdNeverExpires":"0","mcsProvisionADUser":"1","mcsSyncOn":"1","mcsUnlockAccount":"0","mcsExtendExpiration":"0","mcsDisableADAccount":"0","mcsEnableMailbox":"0","mcsExcludeFromTransfer":"0","mcsExistsInAD":"0","mcsmsExchRequireAuthToSendTo":"0","mcsMailboxEnabled":"0","mcsmsExchHideFromAddressLists":"0","CreatedTime":"2022-11-16 16:25:17.680","EmployeeStartDate":"2022-11-16 16:25:19.873","mcsuserAccountControl":"512","Creator":"2340","DomainConfiguration":"2730","Manager":"6589","ObjectID":"37401","AccountName":"bvilla2","Company":"This Ol' House","Department":"Home Repair","DisplayName":"Villa, Bob","Domain":"DAVRISLABS","EmployeeType":"Employee","FirstName":"Bob","JobTitle":"Supervisor","LastName":"Villa","ObjectType":"Person","mcsADDN":"CN=bvilla2,OU=Users,OU=Global,DC=davrislabs,DC=com","mcsUserPrincipalName":"bvilla2@davrislabs.com","mcsEmployeeStatus":"Active"},"AuthorizationWorkflowInstance":{"HybridObjectID":"53945d7d-7f74-429f-95b9-7168189c15eb","CreatedTime":"2022-11-16 16:50:43.417","Creator":"2340","Requestor":"2340","WorkflowDefinition":"4342","Target":"37401","Request":"37408","ObjectID":"37409","DisplayName":"~mcs: Users: Validate create and modify request","ObjectType":"WorkflowInstance","WorkflowStatus":"Completed"},"ActionWorkflowInstance":{"HybridObjectID":"009e797c-cf7f-4279-890c-b64059b87ece","CreatedTime":"2022-11-16 16:50:44.233","Creator":"2340","Requestor":"2340","WorkflowDefinition":"4294","Target":"37401","Request":"37408","ObjectID":"37410","DisplayName":"~mcs: Users: Generate unique attributes","ObjectType":"WorkflowInstance","WorkflowStatus":"Completed"},"DisplayName":"Update to Person:  'Villa, Bob' Request","ObjectType":"Request","Operation":"Put","RequestStatus":"Completed","TargetObjectType":"Person","ServicePartitionName":"IDWEBADMIN.DAVRISLABS.COM","RequestParameter":[{"Calculated":"false","PropertyName":"Company","Value":"This Ol' House","Operation":"Create","Mode":"Modify"}]}'
CreateLogEntry 4121 $log

# Test Record to delete user
$log = '{"HybridObjectID":"db857e9f-7a79-4a4a-81e9-d16ef21dc11b","ObjectType":"Request","Creator":{"HybridObjectID":"7fb2b853-24f0-4498-9534-4e10589723c4","CreatedTime":"2019-11-26 20:26:08.653","ObjectID":"2340","Creator":"2340","DomainConfiguration":"2730","AccountName":"dhodge-admin","DisplayName":"dhodge-admin","Domain":"DAVRISLABS","Email":"","MailNickname":"dhodge-admin","ObjectType":"Person"},"Operation":"Delete","Target":"ef7e1928-14da-416a-99f5-7b33492e05ef","RequestStatus":"Completed","ManagementPolicy":[{"HybridObjectID":"bae9fd09-e2c7-49f9-b4ae-bc22c9bec6f7","Disabled":"0","GrantRight":"1","CreatedTime":"2019-11-26 20:26:08.653","PrincipalSet":"2732","ResourceCurrentSet":"2835","ResourceFinalSet":"2835","ObjectID":"2893","ActionParameter":"*","ActionType":"Delete","Description":"Administration: Administrators can delete non-administrator users","DisplayName":"Administration: Administrators can delete non-administrator users","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"ddeb89d0-eb7f-4634-941e-f85ff48c35c4","Disabled":"0","GrantRight":"1","CreatedTime":"2020-08-18 21:12:20.600","Creator":"2340","PrincipalSet":"2732","ResourceCurrentSet":"2841","ResourceFinalSet":"2841","ObjectID":"4657","ActionParameter":"*","ActionType":["Add","Create","Delete","Modify","Read","Remove"],"DisplayName":"~mcs: Rights: Administrators can perform any action","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"}],"DisplayName":"Delete Person:  'Villa, Bob' Request","CreatedTime":"11/16/2022 4:24:03 PM","TargetObjectType":"Person","CommittedTime":"11/16/2022 4:24:04 PM","RequestParameter":[{"Calculated":"false","Operation":"Delete"}]}'
CreateLogEntry 4121 $log


# Test Record to add new group
#$log = ''
#CreateLogEntry 4121 $log

# Test Record to add new mcsContact
#$log = ''
#CreateLogEntry 4121 $log


# Test Record to add 2 users to Administrators set

# Test Record to remove 2 users from Administrators set
$log = '{"Calculated":"false","Propertyname":"ExplicitMember","Value":"[{\"HybridObjectID\":\"b03f04fb-a830-4d41-afc7-fdfd4b65c0d1\",\"AccountName\":\"chodge\",\"DepartmentNumber\":null,\"DisplayName\":\"Christina Hodge\",\"Domain\":\"DAVRISLABS\"}]","Operation":"Create","Mode":"Add"},{"Calculated":"false","Propertyname":"ExplicitMember","Value":"[{\"HybridObjectID\":\"fe9b6e79-2144-4cfa-a59f-111f4e3caecc\",\"AccountName\":\"dhodge4\",\"DepartmentNumber\":null,\"DisplayName\":\"Daniel Hodge\",\"Domain\":\"DAVRISLABS\"}]","Operation":"Create","Mode":"Add"}'
CreateLogEntry 4121 $log

# Multipart Message
#$log = "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 1 out of 3: {""HybridObjectID"":""2643ad6b-3a59-4157-b2b6-bb4131470881"",""ObjectType"":""Request"","
#CreateLogEntry 4137 $log
#$log = "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 2 out of 3: ""Creator"": {""HybridObjectID"": ""7fb2b853-24f0-4498-9534-4e10589723c4"",""CreatedTime"": ""2019-12-17 01:19:42.633"",""ObjectID"": ""2340"",""Creator"": ""2340"",""DomainConfiguration"": ""2730"",""AccountName"": ""miminstall"",""DisplayName"": ""miminstall"",""Domain"": ""DAVRISLABS"",""Email"": """",""MailNickname"": ""miminstall"",""ObjectType"": ""Person""},"
#CreateLogEntry 4137 $log
#$log = "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 3 out of 3: ""Operation"": ""Put"",  ""Target"": {""HybridObjectID"": ""7a3e5594-48dc-4aa5-b9bb-042153b17c5e"",""Creator"": ""2731"",""ObjectID"": ""4777"",""Manager"": ""84282"",""AccountName"": ""dhodge100"",},""RequestStatus"": ""Completed"",""DisplayName"": ""Update to Person:  'David Hodge' Request"",""CreatedTime"": ""2/13/2020 10:23:08 PM"",""TargetObjectType"": ""Person"",""CommittedTime"": ""2/13/2020 10:23:09 PM""}"
#CreateLogEntry 4137 $log