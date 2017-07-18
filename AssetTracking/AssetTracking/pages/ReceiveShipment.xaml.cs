using AssetTracking.Managers;
using AssetTracking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        ObservableCollection<AssetStatusModel> AssetStatusCollection;
        CancellationTokenSource cts;
        CancellationToken cancellationToken;
        public ReceiveShipment()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            cancellationToken = cts.Token;
            StatusList.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                //We are just de-selecting a row here
                if (e.Item == null) return;
                ((ListView)sender).SelectedItem = null;
            };
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
            try
            {
                List<AssetStatusModel> status = null;
                Dictionary<HttpManager.LinkDeviceResponse, string> statusDetails = null;
                Task.Run(async () =>
                {
                    statusDetails = await GetAssetStatusFromServer(text);
                    status = GetAssetStatus(statusDetails);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loader.IsVisible = false;
                        DateTime shipmentDate = DateTime.Now;
                        string shipmentStatus = "Shipment Status : ";
                        if (status != null && status.Count == 0)
                        {
                            lblStatus.Text = shipmentStatus + "Good";
                            imgStatus.Source = "thumbsup";
                        }
                        else if (status != null && status.Count > 0)
                        {
                            lblStatus.Text = shipmentStatus + "Bad";
                            imgStatus.Source = "thumbsdown";
                            AssetStatusCollection = new ObservableCollection<AssetStatusModel>(status);
                            lblStatusHeading.Text += AssetStatusCollection.Count;
                            StatusList.ItemsSource = AssetStatusCollection;
                            slStatusReasons.IsVisible = true;
                        }
                        else if (status == null)
                        {
                            App.Current.MainPage = new AssetTrackingPage();
                        }
                        lblDate.Text = shipmentDate.ToString("dd/MM/yy");
                        lblTime.Text = shipmentDate.ToString("h:mm tt");
                        ShipmentId.Text = text;
                        ScanningContainer.IsVisible = false;
                        ScannedContainer.IsVisible = true;
                    });
                }, cancellationToken);
            }
            catch (Exception e)
            {
            }
        }

        List<AssetStatusModel> GetAssetStatus(Dictionary<HttpManager.LinkDeviceResponse, string> typeResponse)
        {
            List<AssetStatusModel> status = null;

            if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.Success))
            {
                string statusJson = null;
                typeResponse.TryGetValue(HttpManager.LinkDeviceResponse.Success, out statusJson);
                if (!string.IsNullOrEmpty(statusJson))
                {
                    status = JsonConvert.DeserializeObject<List<AssetStatusModel>>(statusJson);
                }
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
                    string statusMsg;
                    typeResponse.TryGetValue(HttpManager.LinkDeviceResponse.ProcessError, out statusMsg);
                    DisplayAlert("Error Occured !!", statusMsg, "OK");
                    App.Current.MainPage = new AssetTrackingPage();
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
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.InternalError))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Internal Server Error !!", "Error in processing your request.", "OK");
                });
            }
            return status;
        }

        async Task<Dictionary<HttpManager.LinkDeviceResponse, string>> GetAssetStatusFromServer(string assetId)
        {
            string requestStr = JsonConvert.SerializeObject(assetId);
            Dictionary<HttpManager.LinkDeviceResponse, string> typeResponse = await HttpManager.GetInstance().GetAssetStatus(requestStr, cancellationToken);
            return typeResponse;
        }
        async void Back(object sender, EventArgs args)
        {
            if (zxingView != null)
            {
                zxingView.IsEnabled = false;
                zxingView.IsScanning = false;
                ScannerLayout.Children.Remove(zxingView);
                zxingView = null;
            }
            if (cts != null)
            {
                cts.Cancel();
            }
            await Navigation.PopModalAsync();
        }
        async void Done(object sender, EventArgs args)
        {
            if (zxingView != null)
            {
                zxingView.IsEnabled = false;
                zxingView.IsScanning = false;
                ScannerLayout.Children.Remove(zxingView);
                zxingView = null;
            }
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
        protected override bool OnBackButtonPressed()
        {
            this.Back(null, null);
            return true;
        }
    }
}
