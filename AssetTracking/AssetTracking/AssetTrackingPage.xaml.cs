using AssetTracking.pages;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AssetTracking
{
    public partial class AssetTrackingPage : ContentPage
    {
		public AssetTrackingPage()
        {
            InitializeComponent();
			TapGesture.Tapped += OnTappedDispatch;
			TabReceive.Tapped += OnTappedReceive;
        }

         async void OnTappedDispatch(object sender, EventArgs args)
        {
			TapGesture.Tapped -= OnTappedDispatch;
				await Navigation.PushModalAsync(new DispatchShipment());

				TapGesture.Tapped += OnTappedDispatch;
             

        }

        async void OnTappedReceive(object sender, EventArgs args)
        {
			TabReceive.Tapped -= OnTappedReceive;
            await Navigation.PushModalAsync(new ReceiveShipment());
			TabReceive.Tapped += OnTappedReceive;
        }
    }
}
