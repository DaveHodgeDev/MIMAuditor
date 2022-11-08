using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogExtractorConsole
{
    class RequestParameterEntry
    {
        public string Calculated { get; set; }

        public string Propertyname { get; set; }

        public string Value { get; set; }

        public string Operation { get; set; }

        public string Mode { get; set; }
    }

    class RequestParameterEntryReference
    {
        public string HybridObjectID { get; set; }

        public string AccountName { get; set; }

        public string DepartmentNumber { get; set; }

        public string DisplayName { get; set; }

        public string Domain { get; set; }
    }
}
