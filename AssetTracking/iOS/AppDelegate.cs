using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using AssetTracking.iOS;
using AssetTracking.DependencyServices;
using AssetTracking.Managers;
using Microsoft.Identity.Client;

[assembly: Xamarin.Forms.Dependency(typeof(AppDelegate))]
namespace AssetTracking.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate,IAuthentication
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
        void IAuthentication.InitPlatformParameters()
        {
            if (UIApplication.SharedApplication.KeyWindow!=null)
            {
                var platformParameters = UIApplication.SharedApplication.KeyWindow.RootViewController;
                AuthenticationManager.GetInstance().AuthenticationClient.PlatformParameters = new PlatformParameters(platformParameters);
            }            
        }
    }
}
