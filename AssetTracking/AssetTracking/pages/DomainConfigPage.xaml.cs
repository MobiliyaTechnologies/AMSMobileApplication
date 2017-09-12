using AssetTracking.DependencyServices;
using AssetTracking.Managers;
using AssetTracking.Models;
using AssetTracking.Utilities;
using Microsoft.Identity.Client;
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
                DisplayAlert("Error!", "Domain address should not be blank.", "OK");
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
                        B2CConfigManager.GetInstance().Initialize(config);
                        AuthenticationManager.GetInstance().InitializeAuthClient(config.B2cClientId);
                        Application.Current.Properties[Constants.APP_CONFIGURATION_KEY] = jsonResp;
                        try
                        {
                            Dictionary<AuthenticationManager.AuthResponse, object> response = await AuthenticationManager.GetInstance().AquireTokenAsync();
                            if(response.Count!=0)
                            {
                                if(response.ContainsKey(AuthenticationManager.AuthResponse.AuthResult))
                                {
                                    //We might need to save anything for future use
                                   App.Current.MainPage= new AssetTrackingPage();
                                }
                                //We might need to add else if if response condition inreases
                                else
                                {
                                    object errorMsg = null;
                                    response.TryGetValue(AuthenticationManager.AuthResponse.Error, out errorMsg);
                                    if(errorMsg!=null)
                                    {
                                        await DisplayAlert("Authentication Error", errorMsg.ToString(), "OK");
                                    }
                                    else
                                    {
                                        await DisplayAlert("Authentication Error !!", "Error occured while Authentication", "OK");
                                    }           
                                }
                            }

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Loader.IsVisible = false;
                            });
                        }
                        catch (Exception ex)
                        {
                            Loader.IsVisible = false;
                            await DisplayAlert("Authentication Error", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                            DisplayAlert("Error!", "Failed to get all configuration for this domain.", "OK");
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loader.IsVisible = false;
                        DisplayAlert("Error!", "Problem in fetching configuration data from server.Check your domain name.", "OK");
                    });
                }
                
               
            }
        }
    }
}