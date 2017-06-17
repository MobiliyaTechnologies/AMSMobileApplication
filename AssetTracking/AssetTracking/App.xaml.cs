using AssetTracking.pages;
using AssetTracking.Utilities;
using Xamarin.Forms;

namespace AssetTracking
{
	public partial class App : Application
	{
        
       
		public App()
		{
			InitializeComponent();
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

           
			//MainPage = new AbsoluteSample();
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
