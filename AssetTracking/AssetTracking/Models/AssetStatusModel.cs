using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking.Models
{
    public class AssetStatusModel
    {
        public string Capability { get; set; }
        public string CapabilityFilter { get; set; }
        public string MinThreashold { get; set; }
        public string MaxThreashold { get; set; }
        public DateTime Timestamp { get; set; }
        public string Value { get; set; }
    }
}
