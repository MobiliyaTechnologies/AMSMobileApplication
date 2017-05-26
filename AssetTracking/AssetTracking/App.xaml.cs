using Xamarin.Forms;

namespace AssetTracking
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new AssetTrackingPage();
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
