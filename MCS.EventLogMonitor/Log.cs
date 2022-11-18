using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MCS.EventLogMonitor;

namespace MCS.EventLogMonitor
{
    public class Log //: LogBase
    {
        #region Json Properties

        // *********************************************************
        // Event Log entry data
        // *********************************************************
        public DateTime EventGeneratedTime { get; set; }

        public string MachineName { get; set; }

        public string EntryType { get; set; }

        public string Source { get; set; }

        public long EventId { get; set; }

        public string User { get; set; }

        // *********************************************************
        // Request
        // *********************************************************
        public string ObjectType { get; set; }

        public string RequestStatus { get; set; }

        public string Type { get; set; }

        //public string RequestStatusDetail { get; set; }

        // CreatedTime - Time request was created in MIM
        // Already in UTC Format
        public DateTime CreatedTime { get; set; }

        //Already in UTC Format
        public DateTime CommittedTime { get; set; }

        public string DisplayName { get; set; }

        public string HybridObjectId { get; set; }

        public string TargetObjectType { get; set; }

        public string Operation { get; set; }

        public string ParentRequest { get; set; }

        public string Justification { get; set; }

        // *********************************************************
        // Target Settings:
        // *********************************************************
        public string Target_HybridObjectID { get; set; }

        public string Target_ObjectType { get; set; }

        public string Target { get; set; }

        public string Target_AccountName { get; set; }

        public string Target_DisplayName { get; set; }

        public string Target_Department { get; set; }

        public string Target_DepartmentNumber { get; set; }

        public string Target_EmployeeType { get; set; }

        public string Target_EmployeeStatus { get; set; }

        public string Target_ApprovalDuration { get; set; }

        public string Target_Decision { get; set; }

        public string Target_Reason { get; set; }

        public string Target_ApprovalStatus { get; set; }

        public string Reason { get; set; }

        // *********************************************************
        // Creator Settings:
        // *********************************************************
        public string Creator_AccountName { get; set; }

        public string Creator_DisplayName { get; set; }

        public string Creator_HybridObjectID { get; set; }

        // *********************************************************
        // MPR DisplayName
        // *********************************************************
        //public string MPR_DisplayName { get; set; }

        // *********************************************************
        // Approver 
        // *********************************************************
        public string Approver { get; set; }

        public string Approver_AccountName { get; set; }

        public string Approver_DisplayName { get; set; }

        public string Approver_HybridObjectID { get; set; }

        // *********************************************************
        // Request Parameters
        // *********************************************************
        public string RequestParameter { get; set; }

        // *********************************************************
        // Raw Log - original JSON string - no parsing
        // *********************************************************
        // public string RawLog { get; set; }

        public string FileName { get; set; }

        public LogAnalyticsWorkspaceHelper API { get; set; }

        #endregion

        public Log(string filename, LogAnalyticsWorkspaceHelper api)
        {
            FileName = filename;
            API = api;
        }

        /// <summary>
        /// Enrich the log with json input
        /// </summary>
        /// <param name="json"></param>
        public void Augment(string json, bool bFromFile=false)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                //***************************************************************************************************
                // Parse the JSON
                //***************************************************************************************************
                JObject o = JObject.Parse(json);

                //***************************************************************************************************
                // Create concept of categorization into Audit, Approval, or System events for log analytic filtering
                /* Approvals
                    Approval
                    ApprovalResponse
                */

                /* System
                    ActivityInformationConfiguration
                    AttributeTypeDescription
                    BindingDescription
                    Configuration
                    DomainConfiguration
                    ForestConfiguration
                    EmailTemplate
                    FilterScope
                    HomepageConfiguration
                    ma - data
                    mv - data
                    ManagementPolicyRule
                    ObjectTypeDescription
                    NavigationBarConfiguration
                    SearchScopeConfiguration
                    Set
                    SynchronizationFilter
                    SynchronizationRule
                */

                /* Added to System for base object class completeness
                    ConstantSpecifier
                    DetectedRuleEntry
                    ExpectedRuleEntry
                    Function
                    GateRegistration
                    msidmCompositeType
                    msidmDataWarehouseBinding
                    msidmPamConfiguration
                    msidmPamRequest
                    msidmPamRole
                    msidmReportingJob
                    msidmRequestContext
                    msidmRequestTargetDetail
                    msidmSystemConfiguration

                    ObjectVisualizationConfiguration
                    PortalUIConfiguration
                    Request
                    Resource
                    SupportedLocaleConfiguration
                    SystemResourceRetentionConfiguration
                    TimeZoneConfiguration
                    WorkflowDefinition
                    WorkflowInstance
                */

                /* Audit - remaining object classes - approach allows support for customer custom object classes
                    mcsComputer
                    mcsContact
                    mcsDepartment
                    mcsInternetAccessLevel
                    mcsLicense
                    Group
                    Person
                */
                //***************************************************************************************************
                switch (JPathParse(o, "$.TargetObjectType", ""))
                {
                    case "Approval":
                        {
                            this.Type = "Approval";
                            break;
                        }

                    case "ApprovalResponse":
                        {
                            this.Type = "Approval";
                            break;
                        }

                    case "ActivityInformationConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "AttributeTypeDescription":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "BindingDescription":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "Configuration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "ConstantSpecifier":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "DetectedRuleEntry":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "DomainConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "EmailTemplate":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "ExpectedRuleEntry":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "FilterScope":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "ForestConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "Function":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "GateRegistration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "HomepageConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "ManagementPolicyRule":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "ma - data":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "mv - data":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmCompositeType":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmDataWarehouseBinding":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmPamConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmPamRequest":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmPamRole":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmReportingJob":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmRequestContext":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmRequestTargetDetail":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "msidmSystemConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "NavigationBarConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "ObjectTypeDescription":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "ObjectVisualizationConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "PortalUIConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "Request":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "Resource":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "SearchScopeConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "Set":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "SupportedLocaleConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "SynchronizationFilter":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "SynchronizationRule":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "SystemResourceRetentionConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "TimeZoneConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "WorkflowDefinition":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "WorkflowInstance":
                        {
                            this.Type = "System";
                            break;
                        }

                    default:
                        {
                            this.Type = "Audit";
                            break;
                        }
                }

                
                // **********************************************************************
                // Request Settings
                // **********************************************************************
                this.ObjectType = JPathParse(o, "$.ObjectType", "");

                this.RequestStatus = JPathParse(o, "$.RequestStatus", "");

                if (bFromFile == true)
                {
                    this.MachineName = JPathParse(o, "$.MachineName", "");
                    //this.EventGeneratedTime = DateTime.Parse(JPathParse(o, "$.EventGeneratedTime", ""));
                }
                // Evaluate data to see if this should be included
                // this.RequestStatusDetail = JPathParse(o, "$.RequestStatusDetail");

                this.CreatedTime = DateTime.Parse(JPathParse(o, "$.CreatedTime", ""));

                if ((this.RequestStatus != "Failed") && (this.RequestStatus != "Denied"))
                {
                    this.CommittedTime = DateTime.Parse(JPathParse(o, "$.CommittedTime", ""));
                }
                else
                {
                    this.CommittedTime = this.CreatedTime;
                }

                // Request GUID
                this.HybridObjectId = JPathParse(o, "$.HybridObjectID", "");

                // Request DisplayName
                this.DisplayName = JPathParse(o, "$.DisplayName", "");

                this.Operation = JPathParse(o, "$.Operation", "");

                // TargetObjectType is blank on a delete - parse object type from DisplayName
                if (this.Operation == "Delete")
                {
                    // this.DisplayName = "Delete Group:  'Test201 DisplayName' Request"
                    string DisplayName = this.DisplayName;
                    string[] arrDisplayName = DisplayName.Split(' ');
                    string targetObjectType = arrDisplayName[1].Replace(':'.ToString(), "");
                    this.TargetObjectType = targetObjectType;
                }
                else
                {
                    this.TargetObjectType = JPathParse(o, "$.TargetObjectType", "");
                }


                this.ParentRequest = JPathParse(o, "$.ParentRequest", "");

                // **********************************************************************
                //Target Settings
                // Nested objects need detection and special handling for imports
                // this.Target_AccountName = JPathParse(o, "$.Target_AccountName", "");
                // "COMPUTERNAME$"
                

                // **********************************************************************
                this.Target_HybridObjectID = JPathParse(o, "$.Target.HybridObjectID", "");

                if((this.Target_HybridObjectID=="") && (bFromFile == true))
                {
                    this.Target_HybridObjectID = JPathParse(o, "$.Target_HybridObjectID", "");
                }
                
                this.Target_ObjectType = JPathParse(o, "$.Target.ObjectType", "");

                if ((this.Target_ObjectType == "") && (bFromFile == true))
                {
                    this.Target_ObjectType = JPathParse(o, "$.Target_ObjectType", "");
                }

                // **********************************************************************
                // Operation = Delete - GUID of Object
                // **********************************************************************
                if (this.Operation == "Delete")
                {
                    this.Target = JPathParse(o, "$.Target", "");
                }

                this.Target_AccountName = JPathParse(o, "$.Target.AccountName", "");

                if ((this.Target_AccountName == "") && (bFromFile == true))
                {
                    this.Target_AccountName = JPathParse(o, "$.Target_AccountName", "");
                }
                
                this.Target_DisplayName = JPathParse(o, "$.Target.DisplayName", "");

                // **********************************************************************
                // Calculate the DisplayName from the request as it doesn't exist on the object...
                // e.g. Request DisplayName =  "Delete Group:  'Test201 DisplayName' Request"
                // && (this.Operation == "Delete")
                // **********************************************************************
                if ((this.Target_AccountName == "") && (this.Target_DisplayName == ""))
                {
                    if (this.DisplayName.IndexOf("'") != (this.DisplayName.LastIndexOf("'") - 1))
                    {
                        this.Target_DisplayName = this.DisplayName.Substring(this.DisplayName.IndexOf("'") + 1, this.DisplayName.LastIndexOf("'") - this.DisplayName.IndexOf("'") - 1);
                    }
                }

                if ((this.Target_DisplayName == "") && (bFromFile == true))
                {
                    this.Target_DisplayName = JPathParse(o, "$.Target_DisplayName", "");
                }


                this.Target_Department = JPathParse(o, "$.Target.Department", "");

                if ((this.Target_Department == "") && (bFromFile == true))
                {
                    this.Target_Department = JPathParse(o, "$.Target_Department", "");
                }

                this.Target_DepartmentNumber = JPathParse(o, "$.Target.mcsDepartmentNumber", "");

                if ((this.Target_DepartmentNumber == "") && (bFromFile == true))
                {
                    this.Target_DepartmentNumber = JPathParse(o, "$.Target_DepartmentNumber", "");
                }

                this.Target_EmployeeType = JPathParse(o, "$.Target.EmployeeType", "");
                if ((this.Target_EmployeeType == "") && (bFromFile == true))
                {
                    this.Target_EmployeeType = JPathParse(o, "$.Target_EmployeeType", "");
                }

                this.Target_EmployeeStatus = JPathParse(o, "$.Target.mcsEmployeeStatus", "");
                if ((this.Target_EmployeeStatus == "") && (bFromFile == true))
                {
                    this.Target_EmployeeStatus = JPathParse(o, "$.Target_EmployeeStatus", "");
                }

                this.Target_ApprovalDuration = JPathParse(o, "$.Target.ApprovalDuration", "");

                if ((this.Target_ApprovalDuration == "") && (bFromFile == true))
                {
                    this.Target_ApprovalDuration = JPathParse(o, "$.Target_ApprovalDuration", "");
                }

                this.Target_Decision = JPathParse(o, "$.Target.Decision", "");
                if ((this.Target_Decision == "") && (bFromFile == true))
                {
                    this.Target_Decision = JPathParse(o, "$.Target_Decision", "");
                }

                this.Target_Reason = JPathParse(o, "$.Target.Reason", "");
                if ((this.Target_Reason == "") && (bFromFile == true))
                {
                    this.Target_Reason = JPathParse(o, "$.Target_Reason", "");
                }

                this.Target_ApprovalStatus = JPathParse(o, "$.Target.ApprovalStatus", "");
                if ((this.Target_ApprovalStatus == "") && (bFromFile == true))
                {
                    this.Target_ApprovalStatus = JPathParse(o, "$.Target_ApprovalStatus", "");
                }

                // **********************************************************************
                // Requester justification and Approver reason
                // **********************************************************************
                this.Justification = JPathParse(o, "$.Justification", "");
                this.Reason = JPathParse(o, "$.Reason", "");

                // **********************************************************************
                // Creator Settings
                // **********************************************************************
                this.Creator_HybridObjectID = JPathParse(o, "$.Creator.HybridObjectID", "");

                if ((this.Creator_HybridObjectID == "") && (bFromFile == true))
                {
                    this.Creator_HybridObjectID = JPathParse(o, "$.Creator_HybridObjectID", "");
                }

                // Use Creator DisplayName if no accountName set, typically accountname should be present
                string accountName = JPathParse(o, "$.Creator.AccountName", "");
                if (string.IsNullOrEmpty(accountName)) this.Creator_AccountName = JPathParse(o, "$.Creator.DisplayName", "");
                else this.Creator_AccountName = accountName;

                if ((this.Creator_AccountName == "") && (bFromFile == true))
                {
                    this.Creator_AccountName = JPathParse(o, "$.Creator_AccountName", "");
                }

                this.Creator_DisplayName = JPathParse(o, "$.Creator.DisplayName", "");

                if ((this.Creator_DisplayName == "") && (bFromFile == true))
                {
                    this.Creator_DisplayName = JPathParse(o, "$.Creator_DisplayName", "");
                }

                // **********************************************************************
                // MPR Settings
                // **********************************************************************
                // this.MPR_DisplayName = JPathParse(o, "$.ManagementPolicy.DisplayName","");

                // **********************************************************************
                // Approver Parameters
                // **********************************************************************
                this.Approver = JPathParse(o, "$.ComputedActor", "");

                // Supports single level approvers - base call to computedactor would be expandable...
                this.Approver_HybridObjectID = JPathParse(o, "$.ComputedActor", "HybridObjectID");
                if ((this.Approver_HybridObjectID == "") && (bFromFile == true))
                {
                    this.Approver_HybridObjectID = JPathParse(o, "$.Approver_HybridObjectID", "");
                }

                this.Approver_AccountName = JPathParse(o, "$.ComputedActor", "AccountName");
                if ((this.Approver_AccountName == "") && (bFromFile == true))
                {
                    this.Approver_AccountName = JPathParse(o, "$.Approver_AccountName", "");
                }

                this.Approver_DisplayName = JPathParse(o, "$.ComputedActor", "DisplayName");
                if ((this.Approver_DisplayName == "") && (bFromFile == true))
                {
                    this.Approver_DisplayName = JPathParse(o, "$.Approver_DisplayName", "");
                }

                //Justification
                // **********************************************************************
                // Request Parameters
                // **********************************************************************
                this.RequestParameter = JPathParse(o, "$.RequestParameter", "");

                // Rawlog 
                // this.RawLog = json;
            }
            catch (Exception ex)
            {
                MCS.EventLogMonitor.EventLogMonitorConfig config = new EventLogMonitorConfig();

                //Full JSON
                Guid g = Guid.NewGuid();
                string filename = config.jsonFileDirectory + "\\Preprocess-" + g.ToString() + ".json";
                System.IO.File.WriteAllText(filename, json);
                const string EventSource = "MCS.LogAnalytics.Collector";//MCS.HybridReportLogger
                //const string EventLogName = "MCS Azure Monitor Workspace Collector";

                // //Console.WriteLine("API Post Exception: " + excep.Message);
                System.Diagnostics.EventLog.WriteEntry(EventSource, "PreProcess (Preprocess - " + g.ToString() + ".json) - " + ex, System.Diagnostics.EventLogEntryType.Information, 300);

                //throw;
            }
        }

        /// <summary>
        /// Writes log to Log Analytics workspace
        /// </summary>
        public void WriteLog()
        {
            //const string EventSource = "MCS.LogAnalytics.Collector";//MCS.HybridReportLogger
            const string EventLogName = "MCS Azure Monitor Workspace Collector";

            
            // *************************************************************************************
            // EventLog - name of this applications event log
            // *************************************************************************************
            //if (!EventLog.Exists(EventLogName))
            //{
            //    EventLog.CreateEventSource(EventSource, EventLogName);
            //}
                        
            string output = this.SerializeToString();

            if (!string.IsNullOrEmpty(FileName))
            {
                //Console.WriteLine(output);
                using (StreamWriter w = File.AppendText(FileName))
                {
                    w.WriteLine(output);
                    w.WriteLine("-------------------------------");
                }
            }
            if (API != null)
            {
                // Write to the event log for Failed/Denied requests from Synchronization Account
                if ((this.Creator_HybridObjectID == "fb89aefa-5ea1-47f1-8890-abe7797d6497") && ((this.RequestStatus == "Failed") || (this.RequestStatus == "Denied")))
                {
                    if (output.Length <= 30000)
                    {
                        System.Diagnostics.EventLog.WriteEntry(EventLogName, output, System.Diagnostics.EventLogEntryType.Information, 1000);

                    }
                    else
                    {
                        System.Diagnostics.EventLog.WriteEntry(EventLogName, output.Substring(0, 30000), System.Diagnostics.EventLogEntryType.Information, 1000);
                    }
                }

                if (this.EventGeneratedTime.ToString() == "1/1/0001 12:00:00 AM")
                {
                    // Cannot be committed time as that doesn't always exist
                    this.EventGeneratedTime = this.CreatedTime;
                    API.PostData(this.EventGeneratedTime.ToString(), output);
                }
                else
                {
                    API.PostData(this.EventGeneratedTime.ToString(), output);
                }
            }
        }

        public string SerializeToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        private string JPathParse(JObject o, string path, string subString)
        {
            try
            {
                JToken token = o.SelectToken(path, true);

                if (token != null)
                {
                    switch (token.Type.ToString().ToLower())
                    {
                        case "array":
                            {
                                if (path.Contains("$.ComputedActor"))
                                {
                                    // Get's Primary ComputedActor's information
                                    if (subString != "")
                                    {
                                        foreach (var item in token.Children())
                                        {
                                            var itemProperties = item.Children<JProperty>();
                                            var myElement = itemProperties.FirstOrDefault(x => x.Name == subString);
                                            var myElementValue = myElement.Value;
                                            return myElementValue.ToString();
                                        }
                                    }
                                    else
                                    {
                                        return JsonConvert.SerializeObject(token);
                                    }
                                }
                                else if (path.Contains("$.RequestParameter"))
                                {

                                    // ***********************************************************************************
                                    // ResultParameter Entries
                                    // ***********************************************************************************
                                    ArrayList my_RPEs = new ArrayList();

                                    // ***********************************************************************************
                                    // For Each Item in RequestParameters
                                    // ***********************************************************************************
                                    foreach (JObject item in token.Children())
                                    {
                                        var itemProperties = item.Children<JProperty>();

                                        RequestParameterEntry rpe = new RequestParameterEntry();
                                        // USed ArrayList instead - RequestParameterEntryReference rpeReference = new RequestParameterEntryReference();

                                        // ***********************************************************************************
                                        // Enumerate each RequestParameterEntry
                                        // ***********************************************************************************
                                        foreach (var itemproperty in itemProperties)
                                        {
                                            var myElement = itemProperties.FirstOrDefault(x => x.Name == itemproperty.Name);
                                            var myElementValue = myElement.Value;

                                            switch (itemproperty.Name)
                                            {
                                                case "Calculated":
                                                    {
                                                        rpe.Calculated = myElementValue.ToString();
                                                        break;
                                                    }

                                                case "Mode":
                                                    {
                                                        rpe.Mode = myElementValue.ToString();
                                                        break;
                                                    }

                                                case "Operation":
                                                    {
                                                        rpe.Operation = myElementValue.ToString();
                                                        break;
                                                    }

                                                case "PropertyName":
                                                    {
                                                        rpe.Propertyname = myElementValue.ToString();
                                                        break;
                                                    }

                                                case "Value":
                                                    {
                                                        // Value can be either a string or an object
                                                        //    - myElementValue.Type - Object, String
                                                        if (myElementValue.Type.ToString() == "Object")
                                                        {
                                                            var ExplicitMembers = myElementValue.Children<JProperty>();
                                                            RequestParameterEntryReference rpeReference = new RequestParameterEntryReference();
                                                            ArrayList my_RPE_ExplicitMembers = new ArrayList();

                                                            foreach (JProperty member in ExplicitMembers)
                                                            {
                                                                switch (member.Name)
                                                                {
                                                                    case "HybridObjectID":
                                                                        {
                                                                            rpeReference.HybridObjectID = member.Value.ToString();
                                                                            break;
                                                                        }

                                                                    case "Domain":
                                                                        {
                                                                            rpeReference.Domain = member.Value.ToString();
                                                                            break;
                                                                        }

                                                                    case "AccountName":
                                                                        {
                                                                            rpeReference.AccountName = member.Value.ToString();
                                                                            break;
                                                                        }

                                                                    case "DisplayName":
                                                                        {
                                                                            rpeReference.DisplayName = member.Value.ToString();
                                                                            break;
                                                                        }

                                                                    case "mcsDepartmentNumber":
                                                                        {
                                                                            rpeReference.DepartmentNumber = member.Value.ToString();
                                                                            break;
                                                                        }

                                                                    default:
                                                                        {
                                                                            //Do not add to the RequestParamterEntry
                                                                            break;
                                                                        }
                                                                }
                                                            }

                                                            my_RPE_ExplicitMembers.Add(rpeReference);
                                                            rpe.Value = JsonConvert.SerializeObject(my_RPE_ExplicitMembers);
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            //Assumes that the element is a string
                                                            rpe.Value = myElementValue.ToString();
                                                        }

                                                        break;
                                                    }
                                            } // END Switch(itemproperty.Name)
                                        } //END foreach (var itemproperty in itemProperties) 

                                        my_RPEs.Add(rpe);
                                    } //foreach (JObject item in token.Children())

                                    return JsonConvert.SerializeObject(my_RPEs);
                                } //END If "RequestParameter"

                                // If not 
                                return JsonConvert.SerializeObject(token);
                            } // END Case "ARRAY" 

                        default:
                        {
                            return token.Value<string>();
                        }
                    }
                }
                else
                {
                    // When value is empty, return an empty string
                    return string.Empty;
                } //if (token != null)
            }
            catch (Exception)
            {
                // When value is empty, return an empty string
                return string.Empty;
            }
        }
    }
}
