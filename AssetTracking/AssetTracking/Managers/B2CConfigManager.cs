using AssetTracking.Models;
using AssetTracking.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking.Managers
{
    public class B2CConfigManager
    {
        private static B2CConfigManager _instance;
        private B2CConfigManager()
        {

        }
        public static B2CConfigManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new B2CConfigManager();
            }
            return _instance;
        }

        B2CConfiguration configuration;
        public void Initialize(B2CConfiguration config)
        {
            configuration = config;
        }
        
        public string GetB2CAADInstanceUrl()
        {
            string tokenUrl = string.Format(configuration.B2cMobileInstanceUrl, configuration.B2cTenant, configuration.B2cSignUpInPolicyId, configuration.B2cClientId,configuration.B2cMobileRedirectUrl);
            return tokenUrl;
        }
        public string GetB2CTokenUrl(string accessCode)
        {
            string tokenUrl = string.Format(configuration.B2cMobileTokenUrl, configuration.B2cTenant, configuration.B2cSignUpInPolicyId, configuration.B2cClientId, accessCode);
            return tokenUrl;
        }

        public string[] GetB2CScopes()
        {
            string[] arr = { configuration.B2cClientId };
            return arr;
        }

        public string GetB2CAuthority()
        {
            string authority = "https://login.microsoftonline.com/"+configuration.B2cTenant+"/";
            return authority;
        }
        public string GetSignInUpID()
        {
            return configuration.B2cSignUpInPolicyId;
        }


    }
}
