
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BeginWorkPage : ContentPage
    {
        public BeginWorkPage()
        {
            InitializeComponent();

            btnCrewList.Clicked += (s, e) => Navigation.PushAsync(new SelectCrewPage());
            btnEqCheckOut.Clicked += (s, e) => Navigation.PushAsync(new EqCheckOutPage());
            btnEqCheckIn.Clicked += (s, e) => Navigation.PushAsync(new EqCheckInPage());
            btnOdometer.Clicked += (s, e) => PopupNavigation.Instance.PushAsync(new OdometerPopup());


        }

        protected override void OnAppearing()
        {
            

            base.OnAppearing();
        }

        private async void btnLogOut_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}