using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Identity.Client;
using AssetTracking.Droid;
using AssetTracking.DependencyServices;
using AssetTracking.Managers;

[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]
namespace AssetTracking.Droid
{

	[Activity(Label = "AssetTracking.Droid", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait,LaunchMode = LaunchMode.SingleTop)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,IAuthentication
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.Window.RequestFeature(WindowFeatures.ActionBar);
			// Name of the MainActivity theme you had there before.
			// Or you can use global::Android.Resource.Style.ThemeHoloLight
			base.SetTheme(Resource.Style.MyTheme);

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			global::ZXing.Net.Mobile.Forms.Android.Platform.Init();

			LoadApplication(new App());
           
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }

        void IAuthentication.InitPlatformParameters()
        {
            AuthenticationManager.GetInstance().AuthenticationClient.PlatformParameters = new PlatformParameters(Xamarin.Forms.Forms.Context as Activity);
        }
    }
}
