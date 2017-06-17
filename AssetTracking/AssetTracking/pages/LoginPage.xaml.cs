using AssetTracking.Managers;
using AssetTracking.Models;
using AssetTracking.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AssetTracking.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            
                //LoginPageView.IsVisible = false;
                LoginWebView.IsVisible = true;
                Browser.Navigated += Browser_Navigated;
                Browser.Navigating += Browser_Navigating;
                Browser.Source = Constants.B2C_AAD_INSTANCE;
           
        }
        private void signIn_Clicked(object sender, EventArgs e)
        {
           

            //R  Navigation.PushAsync(new NavigationPage(new MainPage()));
            //App.Current.MainPage = new AssetTrackingPage();
        }

        private void Browser_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Loader.IsVisible = true;
            });
        }

        private async void Browser_Navigated(object sender, WebNavigatedEventArgs e)
        {
            
            if (e.Url.Contains("&code="))
            {
                string accessCode = Utility.GetAccessTokenfromQueryString(e.Url, "code");
                

                string responseData = await HttpManager.GetInstance().AuthenticateToken(accessCode);
                if(!string.IsNullOrEmpty(responseData))
                {
                    Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY]  = JsonConvert.DeserializeObject<AccessToken>(responseData).id_token;
                    if(string.IsNullOrEmpty(Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY].ToString()))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                        });
                        App.Current.MainPage = new LoginPage();
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                        });
                        App.Current.MainPage = new AssetTrackingPage();
                    }
                }
                //string code = Common.FunGetValuefromQueryString(url, "code");
                //var preferenceHandler = new PreferenceHandler();
                //preferenceHandler.SetAccessCode(code);

                //string tokenURL = string.Format(B2CConfig.TokenURL, B2CConfig.Tenant, B2CPolicy.SignInPolicyId, B2CConfig.Grant_type, B2CConfig.ClientSecret, B2CConfig.ClientId, code);
                //var response = await InvokeApi.Authenticate(tokenURL, string.Empty, HttpMethod.Post);
                //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                //    string strContent = await response.Content.ReadAsStringAsync();
                //    var token = JsonConvert.DeserializeObject<AccessToken>(strContent);
                //    preferenceHandler.SetToken(token.id_token);

                //}

            }
            Device.BeginInvokeOnMainThread(() =>
            {
                Loader.IsVisible = false;
            });
        }
    }
}
