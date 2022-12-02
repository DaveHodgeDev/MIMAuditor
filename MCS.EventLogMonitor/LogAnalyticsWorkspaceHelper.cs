using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MCS.EventLogMonitor
{
    public class LogAnalyticsWorkspaceHelper
    {
        /// <summary>
        /// Log Analytics workspace ID
        /// </summary>
        public string WorkspaceId { get; }

        /// <summary>
        /// Log Analytics Workspace Key
        /// </summary>
        public string WorkspaceKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public string LogName { get; }

        public LogAnalyticsWorkspaceHelper(string workspaceId, string workspaceKey, string logName)
        {
            WorkspaceId = workspaceId;
            WorkspaceKey = workspaceKey;
            LogName = logName;
        }

        // Build the API signature
        private string BuildSignature(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Uploads data to Log Analytics workspace using HTTP Data Collector API (https://docs.microsoft.com/en-us/azure/azure-monitor/platform/data-collector-api)
        /// </summary>
        /// <param name="date">Optional field to specify the timestamp from the data. If the time field is not specified, Azure Monitor assumes the time is the message ingestion time</param>
        /// <param name="json"></param>
        public void PostData(string timeStampField, string json)
        {
            try
            {
                // Create a hash for the API signature
                var datestring = DateTime.UtcNow.ToString("r"); //rfc1123Date
                                                                //DateTime.Parse(rfc1123Date, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern)
                var jsonBytes = Encoding.UTF8.GetBytes(json);
                string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
                string hashedString = BuildSignature(stringToHash, WorkspaceKey);
                string signature = "SharedKey " + WorkspaceId + ":" + hashedString;
                string url = "https://" + WorkspaceId + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                //client.Timeout
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Log-Type", LogName);
                client.DefaultRequestHeaders.Add("Authorization", signature);
                client.DefaultRequestHeaders.Add("x-ms-date", datestring);
                client.DefaultRequestHeaders.Add("time-generated-field", timeStampField);

                
                System.Net.Http.HttpContent httpContent = new StringContent(json, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Task<System.Net.Http.HttpResponseMessage> response = client.PostAsync(new Uri(url), httpContent);
                System.Net.Http.HttpContent responseContent = response.Result.Content;
                string result = responseContent.ReadAsStringAsync().Result;
                bool c = response.IsCompleted;

                // Test if exception triggered Catch to create a temp file
                // throw new Exception("This is a test...");
            }
            catch (Exception)
            {
                MCS.EventLogMonitor.EventLogMonitorConfig config = new EventLogMonitorConfig();
                Guid g = Guid.NewGuid();
                string filename = config.jsonFileDirectory + "\\Processed-" + g.ToString() + ".json";
                System.IO.File.WriteAllText(filename, json);
                throw;
            }
        }
    }
}