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
        public LoginPage()
        {
            InitializeComponent();

            //LoginPageView.IsVisible = false;
            LoginWebView.IsVisible = true;
            Browser.Navigated += Browser_Navigated;
            Browser.Navigating += Browser_Navigating;
            string aadInstance = B2CConfigManager.GetInstance().GetB2CAADInstanceUrl();
            Browser.Source = aadInstance;

        }
        private void signIn_Clicked(object sender, EventArgs e)
        {


            //R  Navigation.PushAsync(new NavigationPage(new MainPage()));
            //App.Current.MainPage = new AssetTrackingPage();
        }

        private  void Browser_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async() =>
            {
                Loader.IsVisible = true;
				if (e.Url.Contains("&code="))
				{
					string accessCode = Utility.GetAccessTokenfromQueryString(e.Url, "code");


					string responseData = await HttpManager.GetInstance().AuthenticateToken(accessCode);
					if (!string.IsNullOrEmpty(responseData))
					{
						Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY] = JsonConvert.DeserializeObject<AccessToken>(responseData).id_token;
						if (string.IsNullOrEmpty(Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY].ToString()))
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
				
					Loader.IsVisible = false;

				}

            });
        }

        private async void Browser_Navigated(object sender, WebNavigatedEventArgs e)
        {           

            Device.BeginInvokeOnMainThread(() =>
							{
								Loader.IsVisible = false;
							});
        }
    }
}
