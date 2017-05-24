using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AssetTracking
{
	public partial class DispatchShipment : ContentPage
	{
		ZXingScannerView zxingView;

		public DispatchShipment()
		{
			InitializeComponent();
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
			/*		var scanPage = new ZXingScannerPage();

					scanPage.OnScanResult += (result) =>
					{
						// Stop scanning
						scanPage.IsScanning = false;

						// Pop the page and show the result
						Device.BeginInvokeOnMainThread(() =>
						{
							Navigation.PopModalAsync();
							DisplayAlert("Scanned Barcode", result.Text, "OK");
						});
					};

					// Navigate to our scanner page
					await Navigation.PushModalAsync(scanPage); */
		}

		void ScanCode() {
			ContainerLayoutDetails.IsVisible = false;
			ContainerLayoutScanner.IsVisible = true;
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
					ContainerLayoutScanner.IsVisible = false;
					ContainerLayoutDetails.IsVisible = true;
				});
			};

			ScannerLayout.Children.Add(zxingView);
			zxingView.IsEnabled = true;

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
			ScanCode();
		}

		async void Back(object sender, EventArgs args)
		{
			//await DisplayAlert("Clicked!", "Do you want to go Next?", "OK");
			await Navigation.PopModalAsync();
		}
	}
}
