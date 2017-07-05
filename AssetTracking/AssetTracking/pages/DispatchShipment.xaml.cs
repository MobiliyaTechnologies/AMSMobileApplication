using AssetTracking.Managers;
using AssetTracking.Models;
using AssetTracking.pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AssetTracking
{
    public partial class DispatchShipment : ContentPage
    {
        ZXingScannerView zxingView;
        bool stepScanSensor = true;
        bool stepScanShipment = false;
        bool stepLink = false;
        string sensorId = null;
        string shipmentId = null;

        public DispatchShipment()
        {
            InitializeComponent();
            ContainerLayoutScanner.IsVisible = true;
            DispatchLayoutScanner.IsVisible = true;
            ContainerLinking.IsVisible = true;
            ContainerLinked.IsVisible = false;
            ScanCode();
        }

        async void OnTappedScanSensor(object sender, EventArgs args)
        {
            if (!stepScanSensor)
            {
                var answer = await DisplayAlert("Scan Sensor!", "Do you want to scan sensor?", "Yes", "No");
                if (answer)
                {
                    stepScanSensor = true;
                    stepScanShipment = false;
                    stepLink = false;
                    sensorId = null;
                    shipmentId = null;
                    ScanCode();
                }
            }
        }

        async void OnTappedScanShipment(object sender, EventArgs args)
        {
            if (!stepScanShipment)
            {
                if (sensorId == null)
                {
                    await DisplayAlert("Scan Shipment!", "Please scan sensor first.", "Ok");
                    return;
                }
                var answer = await DisplayAlert("Scan Shipment!", "Do you want to scan shipment?", "Yes", "No");
                if (answer)
                {
                    stepScanSensor = false;
                    stepScanShipment = true;
                    stepLink = false;
                    shipmentId = null;
                    ScanCode();
                }
            }
        }

        async void OnTappedLink(object sender, EventArgs args)
        {
            if (!stepLink)
            {
                if (sensorId == null || shipmentId == null)
                {
                    await DisplayAlert("Link", "Please scan sensor and shipment first.", "Ok");
                    return;
                }
            }
        }

        void ScanCode()
        {
            ContainerScanningSteps.IsVisible = true;
            ContainerLayoutScanner.IsVisible = true;
            if (stepScanSensor)
            {
                ImageScanSensor.Source = "scan_sensor_green.png";
                ImageScanShipment.Source = "scan_barcode.png";
                ImageLink.Source = "link.png";
                ImgSensorBg.Source = "step_white.png";
                ImgShipmentBg.Source = "step_grey.png";
                ImgLinkBg.Source = "step_grey.png";
                textScanSensor.TextColor = Color.FromHex("229f7c");
                textScanShipment.TextColor = Color.FromHex("2a3129");
                textLink.TextColor = Color.FromHex("2a3129");
                ContailerLayoutLabel.Text = "Scan Sensor's QR Code";
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
                }
                zxingView.OnScanResult += ZxingView_OnScanResult;
                ScannerLayout.Children.Add(zxingView);
            }
            else if (stepScanShipment)
            {
                ImageScanSensor.Source = "checkmark.png";
                ImageScanShipment.Source = "scan_barcode_green.png";
                ImageLink.Source = "link.png";
                ImgSensorBg.Source = "step_grey.png";
                ImgShipmentBg.Source = "step_white.png";
                ImgLinkBg.Source = "step_grey.png";
                textScanSensor.TextColor = Color.FromHex("2a3129");
                textScanShipment.TextColor = Color.FromHex("229f7c");
                textLink.TextColor = Color.FromHex("2a3129");
                ContailerLayoutLabel.Text = "Scan Shipment's Barcode";
                zxingView.OnScanResult += ZxingView_OnScanResult;
            }
            ContainerLayoutDetails.IsVisible = false;
            ContainerLayoutLink.IsVisible = false;
            ContainerLayoutScanner.IsVisible = true;
            zxingView.IsEnabled = true;
        }

        private void ZxingView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                zxingView.IsEnabled = false;
                ContainerLayoutScanner.IsVisible = false;
                Loader.IsVisible = true;
                Loader.IsRunning = true;
                showScannedData(result.Text);
            });
            zxingView.OnScanResult -= ZxingView_OnScanResult;
        }

        void showScannedData(string data)
        {
            if (stepScanSensor)
            {
                string sensorType = null;
                Dictionary<HttpManager.LinkDeviceResponse, string> sensorDetails = null;
                Task.Run(async () =>
                {
                    sensorDetails = await GetSensorDetailsFromServer(data);
                    sensorType = GetSensorType(sensorDetails);
                    if (sensorType == null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                    {
                        App.Current.MainPage = new AssetTrackingPage();
                    });
                        return;
                    }
                    else if (sensorType == "")
                    {
                        return;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loader.IsVisible = false;
                        ContainerScanningSteps.IsVisible = false;
                        sensorId = data;
                        SensorID.Text = data;
                        SensorType.Text = sensorType;
                        detailImage.Source = "qr_scanned.png";
                        SensorDetails.IsVisible = true;
                        ShipmentDetails.IsVisible = false;
                        ContainerLayoutScanner.IsVisible = false;
                        ContainerLayoutDetails.IsVisible = true;
                        ContainerLayoutLink.IsVisible = false;
                        DispatchTitle.Text = "Sensor Details";
                    });
                });
            }
            else if (stepScanShipment)
            {
                Loader.IsVisible = false;
                ContainerScanningSteps.IsVisible = false;
                shipmentId = data;
                ShipmentId.Text = data;
                detailImage.Source = "barcode_scanned.png";
                SensorDetails.IsVisible = false;
                ShipmentDetails.IsVisible = true;
                ContainerLayoutScanner.IsVisible = false;
                ContainerLayoutDetails.IsVisible = true;
                ContainerLayoutLink.IsVisible = false;
                DispatchTitle.Text = "Shipment Details";
            }
            else if (stepLink)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ContainerScanningSteps.IsVisible = true;
                    //ImageScanSensor.Source = "checkmark.png";
                    ImageScanShipment.Source = "checkmark.png";
                    ImageLink.Source = "link_green.png";
                    ImgSensorBg.Source = "step_grey.png";
                    ImgShipmentBg.Source = "step_grey.png";
                    ImgLinkBg.Source = "step_white.png";
                    //textScanSensor.TextColor = Color.FromHex("2a3129");
                    textScanShipment.TextColor = Color.FromHex("2a3129");
                    textLink.TextColor = Color.FromHex("229f7c");
                    SensorDetails.IsVisible = false;
                    ShipmentDetails.IsVisible = false;
                    ContainerLayoutScanner.IsVisible = false;
                    ContainerLayoutDetails.IsVisible = false;
                    ContainerLayoutLink.IsVisible = true;
                });
            }
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

        void Next(object sender, EventArgs args)
        {
            if (stepScanSensor)
            {
                stepScanSensor = false;
                stepScanShipment = true;
                DispatchTitle.Text = "Scan Shipment";
                stepLink = false;
                ScanCode();
            }
            else if (stepScanShipment)
            {
                stepScanSensor = false;
                stepScanShipment = false;
                stepLink = true;
                DispatchTitle.Text = "Link";
                showScannedData(null);
            }
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
            await Navigation.PopModalAsync();
        }

        void Link(object sender, EventArgs args)
        {
            if (zxingView != null)
            {
                zxingView.IsEnabled = false;
                zxingView.IsScanning = false;
                ScannerLayout.Children.Remove(zxingView);
                zxingView = null;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                Loader.IsVisible = true;
            });
            Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(sensorId))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loader.IsVisible = false;
                        DisplayAlert("Error Occured !!", "Invalid Sensor", "OK");
                    });
                }
                else if (string.IsNullOrWhiteSpace(shipmentId))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loader.IsVisible = false;
                        DisplayAlert("Error Occured !!", "Invalid Shipment", "OK");
                    });
                }
                else
                {
                    LinkInfoModel requestModel = new LinkInfoModel() { AssetBarcode = shipmentId, SensorKey = sensorId };
                    string jsonRequestData = JsonConvert.SerializeObject(requestModel);
                    Dictionary<HttpManager.LinkDeviceResponse, string> linkResponse = await HttpManager.GetInstance().LinkDevice(jsonRequestData);
                    if (linkResponse.ContainsKey(HttpManager.LinkDeviceResponse.Success))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                            ContainerLinking.IsVisible = false;
                            ContainerLinked.IsVisible = true;
                        });
                    }
                    else if (linkResponse.ContainsKey(HttpManager.LinkDeviceResponse.IdFailure))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                            DisplayAlert("Error Occured !!", "This sensor is not registered with us.", "OK");
                        });
                    }
                    else if (linkResponse.ContainsKey(HttpManager.LinkDeviceResponse.ProcessError))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                            string responseMsg = null;
                            linkResponse.TryGetValue(HttpManager.LinkDeviceResponse.ProcessError, out responseMsg);
                            if (responseMsg != null)
                            {
                                DisplayAlert("Error Occured !!", responseMsg, "OK");
                            }
                            else
                            {
                                DisplayAlert("Error Occured !!", "Error while processing your request.", "OK");
                            }

                        });
                    }
                    else if (linkResponse.ContainsKey(HttpManager.LinkDeviceResponse.NetworkException))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                            DisplayAlert("Network Error !!", "Check your connection and Try Again.", "OK");
                        });
                    }
                    else if (linkResponse.ContainsKey(HttpManager.LinkDeviceResponse.InternalError))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Loader.IsVisible = false;
                            DisplayAlert("Internal Server Error !!", "Error in processing your request.", "OK");
                        });
                    }
                }
            });


        }



        async void Done(object sender, EventArgs args)
        {
            //await DisplayAlert("Clicked!", "Do you want to go Next?", "OK");
            await Navigation.PopModalAsync();
        }

        async Task<Dictionary<HttpManager.LinkDeviceResponse, string>> GetSensorDetailsFromServer(string sensorId)
        {
            string requestId = JsonConvert.SerializeObject(sensorId);
            Dictionary<HttpManager.LinkDeviceResponse, string> typeResponse = await HttpManager.GetInstance().GetSensorTypeFromID(requestId);
            return typeResponse;
        }

        string GetSensorType(Dictionary<HttpManager.LinkDeviceResponse, string> typeResponse)
        {
            string sensorType = null;

            if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.Success))
            {
                typeResponse.TryGetValue(HttpManager.LinkDeviceResponse.Success, out sensorType);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Loader.IsVisible = false;
                });
            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.IdFailure))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Loader.IsVisible = false;
                    DisplayAlert("Error Occured !!", "This sensor is not registered with us.", "OK");
                });
            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.ProcessError))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Loader.IsVisible = false;
                    DisplayAlert("Error Occured !!", "Error while processing your request.", "OK");
                });
            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.NetworkException))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Loader.IsVisible = false;
                    DisplayAlert("Network Error !!", "Check your connection and Try Again.", "OK");
                });
            }
            else if (typeResponse.ContainsKey(HttpManager.LinkDeviceResponse.InternalError))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Loader.IsVisible = false;
                    DisplayAlert("Internal Server Error !!", "Error in processing your request.", "OK");
                });
            }
            return sensorType;
        }



    }
}
