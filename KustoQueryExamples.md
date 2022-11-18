
# Instructions to run a Kusto Query (KQL):
1.	Login to Azure Portal (https://portal.azure.com)
2.	Select Log Analytics Workspace
3.	Select the MIMAuditor workspace
4.	Select “View Logs” 

# Kusto (KQL) Queries

The following queries show some examples of how to search for common audting scenarios.  

> Note: There may be a slight delay for the MIM Request History events to appear in the KQL queries result.

## New User KQL Query

MIMHybrid_CL 
| project CreatedTime_t, CommittedTime_t, Type_s, RequestStatus_s, TargetObjectType_s, Operation_s,   
 DisplayName_s, 
Target_DisplayName_s, Target_AccountName_s, Target_DepartmentNumber_s, Target_EmployeeType_s,
Creator_HybridObjectID_g, Creator_DisplayName_s, Creator_AccountName_s, 
Approver_DisplayName_s, Approver_AccountName_s, Reason_s, Justification_s, RequestParameter_s, TimeGenerated, EventId_d  
| where TargetObjectType_s == "Person" and Operation_s == "Create" and RequestStatus_s == "Completed" and TimeGenerated > ago(1h)

## New Contact KQL Query

MIMHybrid_CL 
| project CreatedTime_t, CommittedTime_t, Type_s, RequestStatus_s, TargetObjectType_s, Operation_s,   
 DisplayName_s, 
Target_DisplayName_s, Target_AccountName_s, Target_DepartmentNumber_s, Target_EmployeeType_s,
Creator_HybridObjectID_g, Creator_DisplayName_s, Creator_AccountName_s, 
Approver_DisplayName_s, Approver_AccountName_s, Reason_s, Justification_s, RequestParameter_s, TimeGenerated, EventId_d  
| where TargetObjectType_s == "mcsContact" and Operation_s == "Create" and RequestStatus_s == "Completed" and TimeGenerated > ago(1h)

## New Group KQL Query

MIMHybrid_CL 
| project CreatedTime_t, CommittedTime_t, Type_s, RequestStatus_s, TargetObjectType_s, Operation_s,   
 DisplayName_s, 
Target_DisplayName_s, Target_AccountName_s, 
Creator_HybridObjectID_g, Creator_DisplayName_s, Creator_AccountName_s, 
Approver_DisplayName_s, Approver_AccountName_s, Reason_s, Justification_s, RequestParameter_s, TimeGenerated, EventId_d  
| where TargetObjectType_s == "Group" and Operation_s == "Create" and RequestStatus_s == "Completed" and TimeGenerated > ago(1h)
| order by TimeGenerated asc

## User Modified Query

MIMHybrid_CL 
| project CreatedTime_t, CommittedTime_t, Type_s, RequestStatus_s, TargetObjectType_s, Operation_s,   
 DisplayName_s, 
Target_DisplayName_s, Target_AccountName_s, Target_DepartmentNumber_s, Target_EmployeeType_s,
Creator_HybridObjectID_g, Creator_DisplayName_s, Creator_AccountName_s, 
Approver_DisplayName_s, Approver_AccountName_s, Reason_s, Justification_s, RequestParameter_s, TimeGenerated, EventId_d  
| where TargetObjectType_s == "Person" and Operation_s == "Put" and RequestStatus_s == "Completed" and TimeGenerated > ago(1h)

## Contact Modified Query

MIMHybrid_CL 
| project CreatedTime_t, CommittedTime_t, Type_s, RequestStatus_s, TargetObjectType_s, Operation_s,   
 DisplayName_s, 
Target_DisplayName_s, Target_AccountName_s, Target_DepartmentNumber_s, Target_EmployeeType_s,
Creator_HybridObjectID_g, Creator_DisplayName_s, Creator_AccountName_s, 
Approver_DisplayName_s, Approver_AccountName_s, Reason_s, Justification_s, RequestParameter_s, TimeGenerated, EventId_d  
| where TargetObjectType_s == "mcsContact" and Operation_s == "Put" and RequestStatus_s == "Completed" and TimeGenerated > ago(1h)

## Group Modified Query

MIMHybrid_CL 
| project CreatedTime_t, CommittedTime_t, Type_s, RequestStatus_s, TargetObjectType_s, Operation_s,   
 DisplayName_s, 
Target_DisplayName_s, Target_AccountName_s, 
Creator_HybridObjectID_g, Creator_DisplayName_s, Creator_AccountName_s, 
Approver_DisplayName_s, Approver_AccountName_s, Reason_s, Justification_s, RequestParameter_s, TimeGenerated, EventId_d  
| where TargetObjectType_s == "Group" and Operation_s == "Put" and RequestStatus_s == "Completed" and TimeGenerated > ago(1h)
| order by TimeGenerated asc

## Set Membership change

MIMHybrid_CL
| where TargetObjectType_s == "Set" and Operation_s == "Put" and RequestStatus_s == "Completed"
| extend Request=parse_json(RequestParameter_s) 
| mv-apply Request on (project mem= tostring(Request.Value), attributeName = tostring(Request.Propertyname) ,mode = tostring(Request.Mode))
| extend Members=parse_json(tostring(mem))
| mv-apply Members on (project userDomain = tostring(Members.Domain), userAccount = tostring(Members.AccountName), userDepartmentNumber = tostring(Members.DepartmentNumber),userHybridObjectID = tostring(Members.HybridObjectID) )
| project CommittedTime = CommittedTime_t, SetName =Target_DisplayName_s, attributeName, mode, userDomain, userAccount, userDepartmentNumber, userHybridObjectID, RequestorAccount=Creator_AccountName_s, RequestorDisplayName=Creator_DisplayName_s
| sort by CommittedTime desc, mode asc, userAccount asc 

![image](https://user-images.githubusercontent.com/47575373/202500545-9ef77edb-c430-4658-b851-f979cabcfc55.png)

