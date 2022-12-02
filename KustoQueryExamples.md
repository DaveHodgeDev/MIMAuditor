
# Instructions to run a Kusto Query (KQL):
1.	Login to Azure Portal (https://portal.azure.com)
2.	Select Log Analytics Workspace
3.	Select the MIMAuditor workspace
4.	Select “View Logs” 

# Kusto (KQL) Query examples

The following queries show some examples of how to search for common audting scenarios.  

> Note: There may be a slight delay for the MIM Request History events to appear in the KQL queries result.

## Basic Object Creation example (User Create)

MIMHybrid_CL | 
project RequestCreated = CreatedTime_t, RequestCommitted = CommittedTime_t, Type= Type_s, Status = RequestStatus_s, 
ObjectType = TargetObjectType_s, Operation = Operation_s,
RequestDisplayName = DisplayName_s, ObjectDisplayname = Target_DisplayName_s, ObjectAccountname = Target_AccountName_s, ObjectDepartmentnumber=Target_DepartmentNumber_s, ObjectEmployeetype=Target_EmployeeType_s, 
Creator_DisplayName = Creator_DisplayName_s, Creator_AccountName = Creator_AccountName_s, 
Approver_DisplayName = Approver_DisplayName_s, Approver_AccountName = Approver_AccountName_s, 
Reason = Reason_s, Justification = Justification_s, 
RequestParameter = RequestParameter_s
| where ObjectType == "Person" and Operation == "Create" and Status == "Completed" 

## Basic Object Modification (User modified)

MIMHybrid_CL | 
project RequestCreated = CreatedTime_t, RequestCommitted = CommittedTime_t, Type= Type_s, Status = RequestStatus_s, 
ObjectType = TargetObjectType_s, Operation = Operation_s,
RequestDisplayName = DisplayName_s, ObjectDisplayname = Target_DisplayName_s, ObjectAccountname = Target_AccountName_s, ObjectDepartmentnumber=Target_DepartmentNumber_s, ObjectEmployeetype=Target_EmployeeType_s, 
Creator_DisplayName = Creator_DisplayName_s, Creator_AccountName = Creator_AccountName_s, 
Approver_DisplayName = Approver_DisplayName_s, Approver_AccountName = Approver_AccountName_s, 
Reason = Reason_s, Justification = Justification_s, 
RequestParameter = RequestParameter_s
| where ObjectType == "Person" and Operation == "Put" and Status == "Completed" 

> Note: To look for users that have had their department attribute updated, add **and RequestParameter contains "Department"** to the where clause from the statement above.

## Set Membership change

MIMHybrid_CL
| where TargetObjectType_s == "Set" and Operation_s == "Put" and RequestStatus_s == "Completed"
| extend Request=parse_json(RequestParameter_s) 
| mv-apply Request on (project mem= tostring(Request.Value), attributeName = tostring(Request.Propertyname) ,mode = tostring(Request.Mode))
| extend Members=parse_json(tostring(mem))
| mv-apply Members on (project Domain = tostring(Members.Domain), Account = tostring(Members.AccountName), userDepartmentNumber = tostring(Members.DepartmentNumber), HybridObjectID = tostring(Members.HybridObjectID) )
| project CommittedTime = CommittedTime_t, SetName =Target_DisplayName_s, attributeName, MembershipChange = mode, Domain, Account, userDepartmentNumber, HybridObjectID, RequestorAccount=Creator_AccountName_s, RequestorDisplayName=Creator_DisplayName_s
| sort by CommittedTime desc, MembershipChange asc, Account asc 

![image](https://user-images.githubusercontent.com/47575373/205328770-43cd9171-cd13-4161-ad2e-152253b32c7d.png)

# Modifying the basic queries

The queries listed above can be modified to build reports. Some common examples are listed in the documentation below. Please see the KQL documentation for language syntax [KQL quick reference](https://learn.microsoft.com/en-us/azure/data-explorer/kql-quick-reference) for more information.

## Selecting the object class

The Object Class should match the name of the Object Class in the MIM Service.

- For Users, change the ObjectType field to **ObjectType == "Person"** 
- For Groups, change the ObjectType field to **ObjectType == "Group"** 
- For Contacts, change the ObjectType field to **ObjectType == "mcsContact"** 

## Selecting the operation type

The operation attribute is responsible for determining what action was recorded: Create, Put (Modify), or Delete 

-  **Operation == "Create" ** will return objects that have been created.
-  **Operation == "Put" **  will return objects that have been modified.
-  **Operation == "Delete"** will return objects that have been deleted

## Selecting the requests that did not complete successfully:

The Status attribute contains the status of the request: **Completed, Failed, Denied **
 
- **Status == "Completed"** will return all completed requests
- **Status <> "Completed"** will return all requests that have not successfully completed

## Restricting the returned records

Below are some additional KQL statements that can be added to the where clause to restrict the returned records:

- **and ObjectAccountname == "bvilla"** will limit the results to records where the MIM object that has an AccountName of "bvilla"
- **and RequestCreated > ago(1h)** will limit the returned results to requests within the last hour

> Note: 1h = means one hour, 1d means one day, 1m means one minute

## Adding a friendly name to returned recordset

It is possible to rename the columns to a more meaningful name than the values that are stored in the MIM Request data. 
- **RequestCreated = CreatedTime_t** will provide a friendly name(**RequestCreated**) to the table's column (**CreatedTime_t**)



