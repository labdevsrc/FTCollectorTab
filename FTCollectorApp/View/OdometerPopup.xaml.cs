using FTCollectorApp.Service;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OdometerPopup
    {
        public OdometerPopup()
        {
            InitializeComponent();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            try
            {
                await CloudDBService.PostJobEvent(entryOdometer.Text);


                /* Tab Navigation */
                await DisplayAlert("Job Event", "Job uploade. Please Continue to Site Menu", "CLOSE");
                /*if (answer)
                {
                    if (Session.stage =="A")
                        await Navigation.PushAsync(new AsBuiltDocMenu());
                    if (Session.stage == "I")
                        await Navigation.PushAsync(new MainMenuInstall());
                }*/
                //await PopupNavigation.Instance.PopAsync(true);
            }
            catch
            {
                await DisplayAlert("Error", "Update JobEvent table failed. Check again internet connection", "CLOSE");
            }
            IsBusy = false;
        }
    }
}