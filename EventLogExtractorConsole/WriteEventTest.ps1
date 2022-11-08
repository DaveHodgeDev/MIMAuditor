﻿New-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service”


Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4121 -Message "test"

$log = '{"HybridObjectID":"1643ad6b-3a59-4157-b2b6-bb4131470880","ObjectType":"Request","Creator":{"HybridObjectID":"7fb2b853-24f0-4498-9534-4e10589723c4","CreatedTime":"2019-12-17 01:19:42.633","ObjectID":"2340","Creator":"2340","DomainConfiguration":"2730","AccountName":"svc_miminstall","DisplayName":"svc_miminstall","Domain":"HOSTED","Email":"","MailNickname":"svc_miminstall","ObjectType":"Person"},"Operation":"Delete","Target":"20bff62a-8edc-48ad-9466-43469d5b3658","RequestStatus":"Completed","ManagementPolicy":[{"HybridObjectID":"a595bc30-248d-4232-927a-416fc48686d0","Disabled":"0","GrantRight":"1","CreatedTime":"2019-12-17 01:19:42.633","ResourceCurrentSet":"2805","ResourceFinalSet":"2805","PrincipalSet":"2828","ObjectID":"2866","ActionParameter":["Description","DisplayName","ExpirationTime","AccountName","DisplayedOwner","Domain","DomainConfiguration","Email","ExplicitMember","MailNickname","Filter","MembershipAddWorkflow","MembershipCondition","MembershipLocked","ObjectType","ObjectSID","Owner","Scope","Type","msidmDeferredEvaluation","msidmPamEnabled","msidmPamSourceDomainName","msidmPamSourceGroupName","msidmPamSourceSid","msidmPamUsesSIDHistory","msidmPamPrivOnlyGroup"],"ActionType":["Create","Delete"],"Description":"Group management: Group administrators can create and delete group resources","DisplayName":"Group management: Group administrators can create and delete group resources","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"},{"HybridObjectID":"e45448ed-8211-48ea-acb2-950947e24f53","Disabled":"0","GrantRight":"1","CreatedTime":"2019-12-19 14:17:35.263","Creator":"2340","PrincipalSet":"2732","ResourceCurrentSet":"2841","ResourceFinalSet":"2841","ObjectID":"4573","ActionParameter":"*","ActionType":["Add","Create","Delete","Modify","Read","Remove"],"DisplayName":"~mcs: _LAC - Rights: Administrators can perform any action","ObjectType":"ManagementPolicyRule","ManagementPolicyRuleType":"Request"}],"DisplayName":"Delete Group: ''Test-New-Group'' Request","CreatedTime":"1/24/2020 2:56:21 PM","TargetObjectType":"Group","CommittedTime":"1/24/2020 2:56:22 PM","RequestParameter":[{"Calculated":"false","Operation":"Delete"}]}'
$log

Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4121 -Message $log


Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 1 out of 3: This is part one."
Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 2 out of 3: This is part two."
Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 3 out of 3: This is part three."

Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 1 out of 3: {""HybridObjectID"":""2643ad6b-3a59-4157-b2b6-bb4131470881"",""ObjectType"":""Request"","
Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 2 out of 3: ""Creator"": {""HybridObjectID"": ""7fb2b853-24f0-4498-9534-4e10589723c4"",""CreatedTime"": ""2019-12-17 01:19:42.633"",""ObjectID"": ""2340"",""Creator"": ""2340"",""DomainConfiguration"": ""2730"",""AccountName"": ""svc_miminstall"",""DisplayName"": ""svc_miminstall"",""Domain"": ""HOSTED"",""Email"": """",""MailNickname"": ""svc_miminstall"",""ObjectType"": ""Person""},"
Write-EventLog –LogName "Identity Manager Request Log" –Source “Microsoft.IdentityManagement.Service” –EntryType Information –EventID 4137 -Message "Request 'E633A974-9D82-49F2-BE2C-A5D120912E4D', message 3 out of 3: ""Operation"": ""Put"",  ""Target"": {""HybridObjectID"": ""7a3e5594-48dc-4aa5-b9bb-042153b17c5e"",""Creator"": ""2731"",""ObjectID"": ""4777"",""Manager"": ""84282"",""AccountName"": ""c194568"",},""RequestStatus"": ""Completed"",""DisplayName"": ""Update to Person:  'David Hodge' Request"",""CreatedTime"": ""2/13/2020 10:23:08 PM"",""TargetObjectType"": ""Person"",""CommittedTime"": ""2/13/2020 10:23:09 PM""}"



#http://jsonpath.com/