using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
//using System.Text.Json;

namespace EventLogExtractorConsole
{
    class Log
    {
        #region Json Properties
        // Time the entry was generated in the Event Log, in local time, converted to UTC at later time
        public DateTime EventGeneratedTime { get; set; }

        public long EventId { get; set; }

        public string EntryType { get; set; }

        // Time the entry was written in the Event Log
        //public DateTime TimeWritten { get; set; }

        public string MachineName { get; set; }


        public string Source { get; set; }

        public string User { get; set; }

        //*********************************************************
        // Request
        //*********************************************************



        //public string RequestStatusDetail { get; set; }

        //Already in UTC Format
        public DateTime CreatedTime { get; set; }

        //Already in UTC Format
        public DateTime CommittedTime { get; set; }

        public string DisplayName { get; set; }

        public string HybridObjectId { get; set; }

        public string Justification { get; set; }

        public string ObjectType { get; set; }

        public string Operation { get; set; }

        public string ParentRequest { get; set; }

        public string RequestStatus { get; set; }

        public string TargetObjectType { get; set; }

        public string Type { get; set; }

        //*********************************************************
        // Target Settings:
        //*********************************************************
        public string Reason { get; set; }

        public string Target { get; set; }

        public string Target_AccountName { get; set; }

        public string Target_ApprovalDuration { get; set; }

        public string Target_ApprovalStatus { get; set; }

        public string Target_Department { get; set; }

        public string Target_Decision { get; set; }

        public string Target_DepartmentNumber { get; set; }

        public string Target_DisplayName { get; set; }

        public string Target_EmployeeStatus { get; set; }

        public string Target_EmployeeType { get; set; }

        public string Target_HybridObjectID { get; set; }

        public string Target_ObjectType { get; set; }

        public string Target_Reason { get; set; }

        //*********************************************************
        // Creator Settings:
        //*********************************************************
        public string Creator_AccountName { get; set; }

        public string Creator_DisplayName { get; set; }

        public string Creator_HybridObjectID { get; set; }

        //*********************************************************
        // MPR DisplayName
        //*********************************************************
        //public string MPR_DisplayName { get; set; }

        //*********************************************************
        // Approver 
        //*********************************************************
        public string Approver { get; set; }

        public string Approver_AccountName { get; set; }

        public string Approver_DisplayName { get; set; }

        public string Approver_HybridObjectID { get; set; }

        //*********************************************************
        // Request Parameters
        //*********************************************************
        public string RequestParameter { get; set; }

        //*********************************************************
        // Raw Log
        //*********************************************************
        public string RawLog { get; set; }

        #endregion

        public string FileName { get; set; }

        private LogAnalyticsWorkspaceHelper API { get; set; }

        public Log(string filename, LogAnalyticsWorkspaceHelper api)
        {
            FileName = filename;
            API = api;
        }

        /// <summary>
        /// Enrich the log with json input
        /// </summary>
        /// <param name="json"></param>
        public void Augment(string json)
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

                    case "ForestConfiguration":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "FilterScope":
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

                    case "ObjectTypeDescription":
                        {
                            this.Type = "System";
                            break;
                        }

                    case "NavigationBarConfiguration":
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
                    case "SynchronizationFilter":
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

                    case "ExpectedRuleEntry":
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

                    case "SupportedLocaleConfiguration":
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

                    /* Audit - anything else
                        mcsComputer
                        mcsContact
                        mcsDepartment
                        mcsInternetAccessLevel
                        mcsLicense
                        Group
                        Person
                    */



                    /*
                       case "Group":
                       {
                           this.Type = "Audit";
                           break;
                       }

                       case "mcsComputer":
                       {
                           this.Type = "Audit";
                           break;
                       }

                       case "mcsContact":
                       {
                           this.Type = "Audit";
                           break;
                       }

                       case "mcsDepartment":
                       {
                           this.Type = "Audit";
                           break;
                       }

                       case "mcsLicense":
                       {
                           this.Type = "Audit";
                           break;
                       }

                       case "Person":
                       {
                           this.Type = "Audit";
                           break;
                       }
                   */
                    default:
                        {
                            this.Type = "Audit";
                            break;
                        }
                }

                //**********************************************************************
                // Request Settings
                //**********************************************************************
                this.CommittedTime = DateTime.Parse(JPathParse(o, "$.CommittedTime", ""));

                this.CreatedTime = DateTime.Parse(JPathParse(o, "$.CreatedTime", ""));

                // Request DisplayName
                this.DisplayName = JPathParse(o, "$.DisplayName", "");

                // Request GUID
                this.HybridObjectId = JPathParse(o, "$.HybridObjectID", "");

                this.ObjectType = JPathParse(o, "$.ObjectType", "");

                // TargetObjectType is blank on a delete - parse object type from DisplayName
                if (this.Operation == "Delete")
                {
                    // this.DisplayName = "Delete Group:  'Test DisplayName' Request"
                    string DisplayName = this.DisplayName;
                    string[] arrDisplayName = DisplayName.Split(' ');
                    string targetObjectType = arrDisplayName[1].Replace(':'.ToString(), "");
                    this.TargetObjectType = targetObjectType;
                }
                else
                {
                    this.TargetObjectType = JPathParse(o, "$.TargetObjectType", "");
                }

                this.Operation = JPathParse(o, "$.Operation", "");

                this.ParentRequest = JPathParse(o, "$.ParentRequest", "");

                this.RequestStatus = JPathParse(o, "$.RequestStatus", "");

                // Evaluate data to see if this should be included
                // this.RequestStatusDetail = JPathParse(o, "$.RequestStatusDetail");


                //**********************************************************************
                //Target Settings
                //**********************************************************************
                this.Target_AccountName = JPathParse(o, "$.Target.AccountName", "");

                this.Target_ApprovalDuration = JPathParse(o, "$.Target.ApprovalDuration", "");

                this.Target_ApprovalStatus = JPathParse(o, "$.Target.ApprovalStatus", "");

                //**********************************************************************
                // Creator Settings
                //**********************************************************************
                this.Creator_HybridObjectID = JPathParse(o, "$.Creator.HybridObjectID", "");

                // Use Creator DisplayName if no accountName set, typically accountname should be present
                string accountName = JPathParse(o, "$.Creator.AccountName", "");

                if (string.IsNullOrEmpty(accountName)) this.Creator_AccountName = JPathParse(o, "$.Creator.DisplayName", "");
                else this.Creator_AccountName = accountName;

                this.Creator_DisplayName = JPathParse(o, "$.Creator.DisplayName", "");

                this.Target_Decision = JPathParse(o, "$.Target.Decision", "");

                this.Target_Department = JPathParse(o, "$.Target.Department", "");

                this.Target_DepartmentNumber = JPathParse(o, "$.Target.mcsDepartmentNumber", "");

                this.Target_DisplayName = JPathParse(o, "$.Target.DisplayName", "");

                this.Target_EmployeeType = JPathParse(o, "$.Target.EmployeeType", "");

                this.Target_EmployeeStatus = JPathParse(o, "$.Target.mcsEmployeeStatus", "");

                this.Target_HybridObjectID = JPathParse(o, "$.Target.HybridObjectID", "");

                this.Target_ObjectType = JPathParse(o, "$.Target.ObjectType", "");

                // Operation = Delete - GUID of Object
                if (this.Operation == "Delete")
                {
                    this.Target = JPathParse(o, "$.Target", "");
                }
                //---------------------------------------------------------------------

                if ((this.Target_AccountName == "") && (this.Target_DisplayName == "") && (this.Operation == "Delete"))
                {
                    // this.DisplayName = "Delete Group:  'Test201 DisplayName' Request"
                    this.Target_DisplayName = this.DisplayName.Substring(this.DisplayName.IndexOf("'") + 1, this.DisplayName.LastIndexOf("'") - this.DisplayName.IndexOf("'") - 1);
                }

                this.Target_Reason = JPathParse(o, "$.Target.Reason", "");

                //**********************************************************************
                // Requester justification and Approver reason
                //**********************************************************************
                this.Justification = JPathParse(o, "$.Justification", "");
                this.Reason = JPathParse(o, "$.Reason", "");


                //**********************************************************************
                // MPR Settings
                //**********************************************************************
                // this.MPR_DisplayName = JPathParse(o, "$.ManagementPolicy.DisplayName","");

                //**********************************************************************
                // Approver Parameters
                //**********************************************************************
                this.Approver = JPathParse(o, "$.ComputedActor", "");

                this.Approver_AccountName = JPathParse(o, "$.ComputedActor", "AccountName");

                this.Approver_DisplayName = JPathParse(o, "$.ComputedActor", "DisplayName");

                // Supports single level approvers - base call to computedactor would be expandable...
                this.Approver_HybridObjectID = JPathParse(o, "$.ComputedActor", "HybridObjectID");

                //Justification
                //**********************************************************************
                // Request Parameters
                //**********************************************************************
                this.RequestParameter = JPathParse(o, "$.RequestParameter", "");

                // Rawlog 
                // this.RawLog = json;
            }
            catch (Exception excep)
            {
                Guid g = Guid.NewGuid();
                string filename = Program.jsonFileDirectory + "\\Preprocess-" + g.ToString() + ".json";
                System.IO.File.WriteAllText(filename, json);

                Console.WriteLine("API Post Exception: " + excep.Message);
                //EventLog.WriteEntry(EventSource, ex.Message, EventLogEntryType.Error, 300//, 1);

                throw;
            }
        }

        public void WriteLog()
        {
            string output = this.SerializeToString();
            if (!string.IsNullOrEmpty(FileName))
            {
                Console.WriteLine(output);
                using (StreamWriter w = File.AppendText(FileName))
                {
                    w.WriteLine(output);
                    w.WriteLine("-------------------------------");
                }
            }
            if (API != null)
            {
                // Exclude Synchronization errors that show as Denied from Synchronization Account
                //if ((this.Creator_HybridObjectID == "fb89aefa-5ea1-47f1-8890-abe7797d6497") && ((this.RequestStatus == "Failed") || (this.RequestStatus == "Denied")))
                //{
                // Do nothing as failed and denied do not need to be uploaded
                //}
                //else
                // {
                API.PostData(this.EventGeneratedTime.ToString(), output);
                // }
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

                                    //***********************************************************************************
                                    // ResultParameter Entries
                                    //***********************************************************************************
                                    ArrayList my_RPEs = new ArrayList();

                                    //***********************************************************************************
                                    // For Each Item in RequestParameters
                                    //***********************************************************************************
                                    foreach (JObject item in token.Children())
                                    {
                                        var itemProperties = item.Children<JProperty>();

                                        RequestParameterEntry rpe = new RequestParameterEntry();
                                        //                                RequestParameterEntryReference rpeReference = new RequestParameterEntryReference();

                                        //***********************************************************************************
                                        // Enumerate each RequestParameterEntry
                                        //***********************************************************************************
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
                                                        //myElementValue.Type - Object, String
                                                        if (myElementValue.Type.ToString() == "Object")
                                                        {
                                                            //switch (rpe.Propertyname)
                                                            //{
                                                            //case "DisplayedOwner":
                                                            //{
                                                            //    break;
                                                            //}

                                                            //case "Owner":
                                                            //{
                                                            //    break;
                                                            //}

                                                            //case "Manager":
                                                            //{ break; }

                                                            //case "ComputedMember":
                                                            //{ break; }

                                                            //case "mcsDSO":
                                                            //    { break; }
                                                            //case "mcsLicenseAdmins":
                                                            //    { break; }

                                                            //case "ExplicitMember":
                                                            //{
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
                                                            //}
                                                            //default:
                                                            //    {
                                                            //        rpe.Value = myElementValue.ToString();
                                                            //        break;
                                                            //    }
                                                            // }
                                                        }
                                                        else
                                                        {
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
                    return string.Empty;
                } //if (token != null)
            }
            catch (Exception)
            {
                //throw new Exception(string.Format("JPath Error={0}", path) + ex.Message, ex);
                return string.Empty;
            }
        }
    }

    //public void WriteLogToFile()
    //{
    //    if (!string.IsNullOrEmpty(FileName))
    //    {
    //        string output = this.SerializeToString();
    //        Console.WriteLine(output);
    //        using (StreamWriter w = File.AppendText(FileName))
    //        {
    //            w.WriteLine(output);
    //            w.WriteLine("-------------------------------");
    //        }
    //    }
    //}

    //public void WriteLogToWorkspace()
    //{
    //    if (API != null)
    //    {
    //        API.PostData(this.EventGeneratedTime.ToString(), this.SerializeToString());
    //    }
    //}
}
