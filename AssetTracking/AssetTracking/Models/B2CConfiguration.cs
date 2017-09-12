using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking.Models
{
    public class B2CConfiguration
    {
        public string B2cMobileRedirectUrl { get; set; }
        public string B2cTenant { get; set; }
        public string B2cClientId { get; set; }
        public string B2cSignUpInPolicyId { get; set; }
        public string B2cMobileInstanceUrl { get; set; }
        public string B2cMobileTokenUrl { get; set; }
    }
}
