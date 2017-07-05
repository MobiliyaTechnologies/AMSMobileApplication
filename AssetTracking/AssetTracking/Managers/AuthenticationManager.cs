using AssetTracking.DependencyServices;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AssetTracking.Managers
{
    public class AuthenticationManager
    {
        private static AuthenticationManager _instance;

        private AuthenticationManager()
        {

        }
        public static AuthenticationManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AuthenticationManager();
            }
            return _instance;
        }

        private string _authToken;
        public string AuthToken { get { return _authToken; } }

        public void InitializeAuthClient(string clientId)
        {
            AuthenticationClient = new PublicClientApplication(clientId);
            DependencyService.Get<IAuthentication>().InitPlatformParameters();
        }

        public enum AuthResponse { AuthResult, Error };
        public async Task<Dictionary<AuthResponse, object>> AquireTokenAsync()
        {
            Dictionary<AuthResponse, object> response = new Dictionary<AuthResponse, object>();
            try
            {
                var authenticationResult = await this.AuthenticationClient.AcquireTokenAsync(B2CConfigManager.GetInstance().GetB2CScopes(),
                    "",
                    UiOptions.SelectAccount,
                    string.Empty,
                    null,
                     B2CConfigManager.GetInstance().GetB2CAuthority(),
                     B2CConfigManager.GetInstance().GetSignInUpID());
                _authToken = authenticationResult.Token;
                response.Add(AuthResponse.AuthResult, authenticationResult);
                return response;
            }
            catch (Exception ex)
            {
                response.Add(AuthResponse.Error, ex.Message);
                return response;
            }

        }

        public async Task<Dictionary<AuthResponse, object>> AquireTokenSilentAsync()
        {
            Dictionary<AuthResponse, object> response = new Dictionary<AuthResponse, object>();
            try
            {
                var authenticationResult = await this.AuthenticationClient.AcquireTokenSilentAsync(B2CConfigManager.GetInstance().GetB2CScopes(),
                string.Empty,
                B2CConfigManager.GetInstance().GetB2CAuthority(),
                B2CConfigManager.GetInstance().GetSignInUpID(),
                false);
                _authToken = authenticationResult.Token;
                response.Add(AuthResponse.AuthResult, authenticationResult);
                return response;
            }
            catch (Exception ex)
            {
                response.Add(AuthResponse.Error, ex.Message);
                return response;
            }
        }

        public PublicClientApplication AuthenticationClient { get; set; }

    }
}
