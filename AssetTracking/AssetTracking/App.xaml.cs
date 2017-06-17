using AssetTracking.Managers;
using AssetTracking.Models;
using AssetTracking.pages;
using AssetTracking.Utilities;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AssetTracking
{
	public partial class App : Application
	{
        
       
		public App()
		{
			InitializeComponent();

            if (!Application.Current.Properties.ContainsKey(Constants.APP_DOMAIN_URL_KEY))
            {
                MainPage = new DomainConfigPage();
            }
            else
            {
                if(string.IsNullOrEmpty(Application.Current.Properties[Constants.APP_DOMAIN_URL_KEY].ToString()))
                {
                    MainPage = new DomainConfigPage();
                }
                else if(!Application.Current.Properties.ContainsKey(Constants.APP_CONFIGURATION_KEY))
                {
                    MainPage = new DomainConfigPage();
                }
                else if(string.IsNullOrEmpty(Application.Current.Properties[Constants.APP_CONFIGURATION_KEY].ToString()))
                {
                    MainPage = new DomainConfigPage();
                }
                else
                {
                    B2CConfiguration config = JsonConvert.DeserializeObject<B2CConfiguration>(Application.Current.Properties[Constants.APP_CONFIGURATION_KEY].ToString());
                    B2CConfigManager.GetInstance().Initialize(config);

                    if (!Application.Current.Properties.ContainsKey(Constants.APP_SETTINGS_ACCESS_TOKEN_KEY))
                    {
                        Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY] = "";
                    }
                    if (string.IsNullOrEmpty(Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY].ToString()))
                    {
                        MainPage = new LoginPage();
                    }
                    else
                    {
                        MainPage = new AssetTrackingPage();
                    }
                }

                
            }
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
