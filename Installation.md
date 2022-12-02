# **MIM Auditor**

To deploy the MIM Auditor, the following high level tasks must be performed:
- Download and compile the MIM Auditor source code
- Create the Log Analytics Workspace
- Install the service on each server with the MIM Service installed

## **Prerequisites**
1. Ensure that .Net 4.7.2 is installed on the server

## **Preparing for installation**

1. Download the latest version of the MIM Auditor (source code or compiled binaries)

2. Compile the code to the latest version or download the compiled version

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
	- Run the **Enable_Hybrid_Reporting.ps1** script 

3. Run MIM Auditor's **Setup.exe** as an Administrator 
	- Add the workspaceID from the Log Analytics Workspace deployment
    
4. Run the Encrypt utility
	- Launch a PowerShell window as an Administrator
	- Switch the root of C: by typing: **CD\**
	- Switch the default location of MIM Auditor by typing: **CD 'C:\Program Files\Microsoft\MIM Auditor'** 
	- Encrypt the Workspace Key by typing: **.\Encrypt.exe -certificate localhost -string <Workspace Key>**
	
	![image](https://user-images.githubusercontent.com/47575373/205304471-e80cddd3-6280-472c-adcb-b0f02a0733da.png)

	> **Note:** The utility will encrypt the workspace key and update the configuration files for the Auditor service and scavenger utility. 

	> **Note:** The encrypt utility uses the certificate defined in the .config file. **If the certificate is refreshed or if the workspace key is refreshed, the utility should be run again. **. 
	
	> **Note:** By default, the localhost certificate is used. However, the site's SSL certificate could be used to standardize when the workspace key encrypted value should be refreshed. 

5. Configure the MIM Service to have a dependency on the MIM Auditor service to prevent the MIM Service from running when the MIM Auditor is no longer running. 
	- Launch a PowerShell window as an Administrator
	- Set the dependency by typing: **Set-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\FIMService" -Name DependOnService -Value @("MIMAUDITOR")**
	- Reboot the server

	![image](https://user-images.githubusercontent.com/47575373/205305791-21e259f8-61a3-4629-9771-ff4325aaf464.png)

	> **Note:** It is important to prevent the MIM Service from issuing requests while the MIM Auditor is not watching for the log events to prevent coverage gaps. 
	
6. Start the MIMAuditor service
	- Logon to the server as an Administrator (if you had rebooted in the previous step)
	- Launch a PowerShell window as an Administrator
	- Validate/Start the Service: Start-Service -Name "MIMAuditor"

7. Create an event and validate that the event is sent to the Log Analytics Workspace

8. Create a scheduled task to run the Scavenger utility to process any requests that may not have been written to the Log Analytics Workspace. Any failure to write the log successfully to the Log Analytics Workspace will be captured to the **C:\Program Files\Microsoft\MIM Auditor\Requests** folder

9. Repeat steps 1 to 8 on each of your MIM Service Servers.
