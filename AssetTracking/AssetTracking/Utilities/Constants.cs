using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking.Utilities
{
    public class Constants
    {
        public static string B2C_TENANT = "AssetMonitoringB2C.onmicrosoft.com";
        public static string B2C_CLIENT_ID = "a2ecd455-5f2a-4329-adae-fe8eb438f53e";
        public static string B2C_SIGN_UP_IN_POLICY_ID = "B2C_1_SiUpIn";
        public static string B2C_REDIRECT_URL = "am://assetmonitoring/redirecturl/";
       // public static string B2C_AAD_INSTANCE = "https://login.microsoftonline.com/"+ B2C_TENANT + "/v2.0/.well-known/openid-configuration?p=" + B2C_SIGN_UP_IN_POLICY_ID;
        public static string B2C_AAD_INSTANCE = "https://login.microsoftonline.com/"+ B2C_TENANT + "/oauth2/v2.0/authorize?p="+ B2C_SIGN_UP_IN_POLICY_ID + "&client_id="+ B2C_CLIENT_ID + "&response_type=code&redirect_uri="+ B2C_REDIRECT_URL + "&response_mode=query&scope=openid&state=arbitrary_data_you_can_receive_in_the_response";

        public static string B2C_AUTH_GRANT_TYPE = "authorization_code";
        public static string B2C_CLIENT_SECRET = "q0tE6*y^l=|$SxJi";
        public static string B2C_AUTH_TOKEN_URL = "https://login.microsoftonline.com/"+ B2C_TENANT + "/oauth2/v2.0/token?p="+ B2C_SIGN_UP_IN_POLICY_ID + "&grant_type="+ B2C_AUTH_GRANT_TYPE +"&client_id="+ B2C_CLIENT_ID + "&code={0}";

        public static string APP_SETTINGS_ACCESS_TOKEN_KEY = "AccessToken";

        public static string API_BASE_ADDRESS = "http://assetmonitoring.azurewebsites.net";
    }
}
