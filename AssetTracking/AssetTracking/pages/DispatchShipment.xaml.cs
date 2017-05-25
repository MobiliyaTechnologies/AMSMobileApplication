using System;
using System.Collections.Generic;

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
			ContainerLinking.IsVisible = true;
			ContainerLinked.IsVisible = false;
			ScanCode();
		}

		async void OnTappedLink(object sender, EventArgs args)
		{
			await DisplayAlert("Clicked!", "Do you want to link?", "OK");
		}

		async void OnTappedScanShipment(object sender, EventArgs args)
		{
			await DisplayAlert("Clicked!", "Do you want to scan shipment?", "OK");
		}

		async void OnTappedScanSensor(object sender, EventArgs args)
		{
			await DisplayAlert("Clicked!", "Do you want to Scan Sensor?", "OK");
		}

		void ScanCode() {
			if (stepScanSensor || zxingView == null)
			{
				ImageScanSensor.Source = "scan_sensor_green.png";
				ImageScanShipment.Source = "scan_barcode.png";
				ImageLink.Source = "link.png";
				textScanSensor.TextColor = Color.FromHex("229f7c");
				textScanShipment.TextColor = Color.FromHex("2a3129");
				textLink.TextColor = Color.FromHex("2a3129");
				ContailerLayoutLabel.Text = "Scan Sensor's QR Code";
				zxingView = new ZXingScannerView
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					AutomationId = "zxingScannerView",
				};
				zxingView.OnScanResult += (result) =>
				{
					// Stop scanning
					//zxingView.IsScanning = false;

					// Pop the page and show the result
					Device.BeginInvokeOnMainThread(() =>
						{
						//Navigation.PopModalAsync();
						//DisplayAlert("Scanned Barcode", result.Text, "OK");

						zxingView.IsEnabled = false;
							showScannedData(result.Text);
						});
				};

				ScannerLayout.Children.Add(zxingView);
			}
			else if (stepScanShipment) 
			{
				ImageScanSensor.Source = "checkmark.png";
				ImageScanShipment.Source = "scan_barcode_green.png";
				ImageLink.Source = "link.png";
				textScanSensor.TextColor = Color.FromHex("2a3129");
				textScanShipment.TextColor = Color.FromHex("229f7c");
				textLink.TextColor = Color.FromHex("2a3129"); 
				ContailerLayoutLabel.Text = "Scan Shipment's Barcode";
			}
			ContainerLayoutDetails.IsVisible = false;
			ContainerLayoutLink.IsVisible = false;
			ContainerLayoutScanner.IsVisible = true;

			zxingView.IsEnabled = true;

		}

		void showScannedData(string data)
		{
			if (stepScanSensor)
			{
				sensorId = data;
				SensorID.Text = data;
				detailImage.Source = "qr_scanned.png";
				SensorDetails.IsVisible = true;
				ShipmentDetails.IsVisible = false;
				ContainerLayoutScanner.IsVisible = false;
				ContainerLayoutDetails.IsVisible = true;
				ContainerLayoutLink.IsVisible = false;

			}
			else if (stepScanShipment)
			{
				shipmentId = data;
				ShipmentId.Text = data;
				detailImage.Source = "barcode_scanned.png";
				SensorDetails.IsVisible = false;
				ShipmentDetails.IsVisible = true;
				ContainerLayoutScanner.IsVisible = false;
				ContainerLayoutDetails.IsVisible = true;
				ContainerLayoutLink.IsVisible = false;
			}
			else if (stepLink) 
			{
				ImageScanSensor.Source = "checkmark.png";
				ImageScanShipment.Source = "checkmark.png";
				ImageLink.Source = "link_green.png";
				textScanSensor.TextColor = Color.FromHex("2a3129");
				textScanShipment.TextColor = Color.FromHex("2a3129");
				textLink.TextColor = Color.FromHex("229f7c");

				SensorDetails.IsVisible = false;
				ShipmentDetails.IsVisible = false;
				ContainerLayoutScanner.IsVisible = false;
				ContainerLayoutDetails.IsVisible = false;
				ContainerLayoutLink.IsVisible = true;
				
			}

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			zxingView.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			zxingView.IsScanning = false;

			base.OnDisappearing();
		}

		void Next(object sender, EventArgs args)
		{
			//await DisplayAlert("Clicked!", "Do you want to go Next?", "OK");
			if (stepScanSensor)
			{
				stepScanSensor = false;
				stepScanShipment = true;
				stepLink = false;
				ScanCode();
			}
			else if (stepScanShipment)
			{
				stepScanSensor = false;
				stepScanShipment = false;
				stepLink = true;
				showScannedData(null);
			}
		}

		async void Back(object sender, EventArgs args)
		{
			//await DisplayAlert("Clicked!", "Do you want to go Next?", "OK");
			await Navigation.PopModalAsync();
		}

		void Link(object sender, EventArgs args)
		{

			ContainerLinking.IsVisible = false;
			ContainerLinked.IsVisible = true;
			//await DisplayAlert("Clicked!", "Do you want Link Sensor and Shipment?", "OK");
		}

		async void Done(object sender, EventArgs args)
		{
			//await DisplayAlert("Clicked!", "Do you want to go Next?", "OK");
			await Navigation.PopModalAsync();
		}
	}
}
