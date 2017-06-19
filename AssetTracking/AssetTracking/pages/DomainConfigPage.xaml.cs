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
    public partial class DomainConfigPage : ContentPage
    {
        public DomainConfigPage()
        {
            InitializeComponent();
            btnSubmit.Clicked += BtnSubmit_Clicked;

        }

        private async void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            string domainName = txtDomain.Text;
           if (string.IsNullOrEmpty(domainName))
            {
                DisplayAlert("Error!", "Domain address should not be null", "OK");
            }
           else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Loader.IsVisible = true;
                });

                Application.Current.Properties[Constants.APP_DOMAIN_URL_KEY] = domainName;
                string jsonResp = await HttpManager.GetInstance().GetB2CConfiguration(Application.Current.Properties[Constants.APP_DOMAIN_URL_KEY].ToString());
                if(jsonResp!=null)
                {
                    B2CConfiguration config = JsonConvert.DeserializeObject<B2CConfiguration>(jsonResp);
                    if (config.B2cClientId != null && config.B2cMobileInstanceUrl != null && config.B2cMobileRedirectUrl != null && config.B2cMobileTokenUrl != null && config.B2cSignUpInPolicyId != null && config.B2cTenant != null)
                    {
                        Application.Current.Properties[Constants.APP_CONFIGURATION_KEY] = jsonResp;
                        B2CConfigManager.GetInstance().Initialize(config);
                        App.Current.MainPage = new LoginPage();
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                            DisplayAlert("Error!", "Fetched data has some null values for B2C login", "OK");
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loader.IsVisible = false;
                        DisplayAlert("Error!", "Problem in fetching proper data from server", "OK");
                    });
                }
                
               
            }
        }
    }
}