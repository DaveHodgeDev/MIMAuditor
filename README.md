# MIMAuditor
MIM Auditor - modernized version of MIM Reporting using Azure technologies.  

MIM Auditor processes the Request History information from the Identity Manager Request Log eventlog and writes the data to an Azure Log Analytics Workspace. 

The solution utilizes the Hybrid Reporting feature of the MIM Service to write Request History events to the event log. MIM Auditor runs as a windows service that monitors the log for new events. When a new event is written to the logs, the code reads the event and writes the data to a Log Analytics Workspace.

Since the data is stored in Log Analytics Workspace, it offers a number of new possibilies for an organization to explore:
  - Kusto Query Language (KQL) provides a rich reporting capability for generating reports about administrative activity within the MIM Service
  - Queries can staged within a workbook and published to a Dashboard for organizational consumption
  - Queries can be saved within the log analytics workspace for future use
  - Report data can be exported from the report to an Excel spreadsheet
  - Log Analytic Workspaces can store request history data up to 730 days
  - No additional on-premises software or hardware is required
  - Azure Monitor Alerts can be generated for administrative events such as changes to the Administrators set

## **How is MIM Auditor licensed?**

The MIM Auditor solution is published under the MIT License as listed in the license.md file.

## **How is MIM Auditor supported?**

Customers should treat this as custom code with source code made available via the MIM Auditor GitHub Repository. Requests for new features or issue remediation should be made at the MIM Auditor GitHub repository. 

## **Contributor Covenant Code of Conduct**
Please be respectful of others and adhere to Contributor Covenant Code of Conduct as described at the following location: https://www.contributor-covenant.org/version/2/0/code_of_conduct/.


# **Below are screenshots of some of the solution capabilities:**

## **MIM Auditor Workbook**

Graphical and data grid reports can be generated to show administrative activity within the organization.
![image](https://user-images.githubusercontent.com/47575373/202265530-7a8e4c4a-e839-4036-b91d-e806a363140e.png)

## **MIM Request information**

An example of a person object being created. 

> **Note:** There are multiple images for the same request

![image](https://user-images.githubusercontent.com/47575373/202266575-2150d641-e732-4218-ba22-de95e0a13a23.png)
![image](https://user-images.githubusercontent.com/47575373/202266697-ce554427-f288-41e5-8a86-e39a8bb41a0f.png)

## **MIM Auditor Azure Monitor Alerts**

Email alerts can be triggered when a user is added to an Administrative set in the MIM Service.
![image](https://user-images.githubusercontent.com/47575373/202265836-03c708e0-df76-457e-ab50-ccb7018755cd.png)

