using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking.Models
{
    public class AccessToken
    {
        public string id_token { get; set; }
        public string token_type { get; set; }
        public string not_before { get; set; }
        public string id_token_expires_in { get; set; }
        public string profile_info { get; set; }

    }
}
