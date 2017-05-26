using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AssetTracking
{
	public partial class Samples : ContentPage
	{
		public Samples()
		{
			InitializeComponent();
		}

		void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
		{
			//valueLabel.Text = args.NewValue.ToString("F3");
			valueLabel.Text = ((Slider)sender).Value.ToString("F3");
		}

		async void OnButtonClicked(object sender, EventArgs args)
		{
			Button button = (Button)sender;
			await DisplayAlert("Clicked!",
				"The button labeled '" + button.Text + "' has been clicked",
				"OK");
		}
	}
}
