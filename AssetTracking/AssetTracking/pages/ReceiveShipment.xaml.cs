using AssetTracking.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace AssetTracking.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiveShipment : ContentPage
    {
        ZXingScannerView zxingView;
        public ReceiveShipment()
        {
            InitializeComponent();
            ScanRecieverCode();
        }
        void ScanRecieverCode()
        {
            if (zxingView == null)
            {
                zxingView = new ZXingScannerView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    AutomationId = "zxingScannerView",
                };
                zxingView.AutoFocus();
                zxingView.BackgroundColor = Color.White;
                zxingView.OnScanResult += ZxingView_OnScanResult;
                ScannerLayout.Children.Add(zxingView);
            }
        }

        private void ZxingView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //To Do
                //Code to call api and give proper response
                zxingView.IsEnabled = false;
                ScanningContainer.IsVisible = false;
                Loader.IsVisible = true;
                Loader.IsRunning = true;
                showScannedData(result.Text);
            });
           
            
            zxingView.OnScanResult -= ZxingView_OnScanResult;
        }

        private void showScannedData(string text)
        {
            string status = null;
            Dictionary<HttpManager.LinkDeviceResponse, string> statusDetails = null;
            Task.Run(async () =>
            {
                statusDetails = await GetAssetStatusFromServer(text);
                status = GetAssetStatus(statusDetails);

                Device.BeginInvokeOnMainThread(() =>
                {
                    Loader.IsVisible = false;

                    if (!string.IsNullOrEmpty(status))
                    {
                        string shipmentStatus = "Shipment Status : ";
                        DateTime shipmentDate = DateTime.Now;
                        if (Convert.ToBoolean(status))
                        {
                            lblStatus.Text = shipmentStatus + "Good";
                            imgStatus.Source = "thumbsup";
                        }
                        else
                        {
                            lblStatus.Text = shipmentStatus + "Bad";
                            imgStatus.Source = "thumbsdown";
                        }
                        lblDate.Text = shipmentDate.ToString("dd/MM/yy");
                        lblTime.Text = shipmentDate.ToString("h:mm tt");
                        ShipmentId.Text = text;
                        ScanningContainer.IsVisible = false;
                        ScannedContainer.IsVisible = true;
                    }
                    else if (status == null)
                    {
                        App.Current.MainPage = new AssetTrackingPage();
                    }

                });
            });

            
        }

        string GetAssetStatus(Dictionary<HttpManager.LinkDeviceResponse, string> typeResponse)
        {
            string status = null;

            if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.Success))
            {
                typeResponse.TryGetValue(HttpManager.LinkDeviceResponse.Success, out status);                
            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.IdFailure))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Error Occured !!", "This asset is not registered with us.", "OK");
                    App.Current.MainPage = new AssetTrackingPage();
                });
            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.ProcessError))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Error Occured !!", "Error while processing your request.", "OK");
                    App.Current.MainPage = new AssetTrackingPage();
                });
            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.UnAuthorize))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    bool action = await DisplayAlert("Un-Authorized !!", "Would you like to Sign-in again ?", "OK", "Cancel");
                    if (action)
                    {
                        App.Current.MainPage = new LoginPage();
                        status = "";
                    }
                    else
                    {
                        App.Current.MainPage = new AssetTrackingPage();
                        status = "";
                    }
                });

            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.NetworkException))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Network Error !!", "Check your connection and Try Again.", "OK");
                    App.Current.MainPage = new AssetTrackingPage();
                });
            }
            return status;
        }

        async Task<Dictionary<HttpManager.LinkDeviceResponse, string>> GetAssetStatusFromServer(string assetId)
        {
            string requestStr = JsonConvert.SerializeObject(assetId);
            Dictionary<HttpManager.LinkDeviceResponse, string> typeResponse = await HttpManager.GetInstance().GetAssetStatus(requestStr);
            return typeResponse;
        }

        async void Back(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            zxingView.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            if (zxingView != null)
            {
                zxingView.IsScanning = false;
            }
            base.OnDisappearing();
        }
    }
}
