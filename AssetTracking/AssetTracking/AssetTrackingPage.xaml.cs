using System;
using Xamarin.Forms;

namespace AssetTracking
{
	public partial class AssetTrackingPage : ContentPage
	{
		public AssetTrackingPage()
		{
			InitializeComponent();
		}

		async void OnTappedDispatch(object sender, EventArgs args) { 
			//await DisplayAlert("Clicked!", "Do you want to dispatch shipment?", "OK");
			await Navigation.PushModalAsync	(new DispatchShipment());
		}

		async void OnTappedReceive(object sender, EventArgs args)
		{
			await DisplayAlert("Clicked!", "Do you want to receive shipment?", "OK");
		}
	}
}
