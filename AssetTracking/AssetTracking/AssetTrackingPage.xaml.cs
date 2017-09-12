using AssetTracking.pages;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using AssetTracking.Managers;

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
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await HttpManager.GetInstance().ReAuthorize();
        }
    }
}
