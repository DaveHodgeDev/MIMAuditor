# **MIM Auditor**

To deploy the MIM Auditor, the following high level tasks must be performed:
- Download and compile the MIM Auditor source code
- Create the Log Analytics Workspace
- Install the service on each server with the MIM Service installed

## **Preparing for installation**

1. Download the latest version of the MIM Auditor 

2. Compile the code to the latest version

## **Create the Log Analytics Workspace**

These following steps can be used for a default configuration of the Log Analytics Workspace. 

> Note: Please use the Location and SKU settings for your organization. 

1. Login to the Azure Portal (https://portal.azure.com )

2. Open a "Cloud Shell" (PowerShell) from the Portal

3. Create a new Resource Group (MIMAuditor)
	- New-AzResourceGroup -Name 'RG-MIMAUDITOR' -Location 'East US'

4. Create a new Log Analytics Workspace (MIMAuditor). Associate the workspace with the new Resource Group. Save the Customer ID and Workspace Key for later usage.
	- New-AzOperationalInsightsWorkspace -Location 'East US' -Name 'MIM-Auditor' -Sku standard -ResourceGroupName 'RG-MIMAUDITOR'
	- Get-AzOperationalInsightsWorkspace -Name MIM-AUDITOR -ResourceGroupName RG-MIMAuditor | Select CustomerID
	- Get-AzOperationalInsightsWorkspace -Name MIM-AUDITOR -ResourceGroupName RG-MIMAuditor | Get-AzOperationalInsightsWorkspaceSharedKeys

5. Adjust the retention days (Log Analytics workspaces->MIM-Auditor->Usage and Estimated Costs). By default, log analytics workspace data will only be retained for 30 days.
	- Select the Data Retention (Days) in the right pane of the console to the customer's retention requirements


## **Install the service**

1. Login to the MIM Service server as the MIM Installer account (Local Administrator)

2. Enable Hybrid Reporting Request Logging
	- Launch a PowerShell window as an Administrator
	- Run the Enable_Hybrid_Reporting.ps1 script 

3. Run Setup.exe as an Administrator to install MIM Auditor 
	- Add the workspaceID and WorkspaceKey from the Log Analytics Workspace deployment
    
4. Start the MIMAuditor service
	- Launch a PowerShell window as an Administrator
	- Start-Service -Name "MIMAuditor"

5. Create an event and validate that the event is sent to the Log Analytics Workspace

6. Repeat steps 1 to 5 on each of your MIM Service Servers.
