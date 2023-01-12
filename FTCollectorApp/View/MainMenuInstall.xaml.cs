using FTCollectorApp.Model;
using FTCollectorApp.Utils;
using FTCollectorApp.View.CablePages;
using FTCollectorApp.View.FiberPages;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.TraceFiberPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuInstall : ContentPage
    {
        bool toggleMenu = false;
        public MainMenuInstall()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            if (Session.Result.Equals("CreateSiteOK"))
            {
                toggleMenu = true;
            }

            btnNewCable.IsEnabled = toggleMenu;
            btnPullCable.IsEnabled = toggleMenu;
            btnSpliceCable.IsEnabled = toggleMenu;
            btnTerminateCable.IsEnabled = toggleMenu;
            btnTraceFiber.IsEnabled = toggleMenu;
            btnInstallDev.IsEnabled = true;
            btnOTDRtest.IsEnabled = true;
            btnInstallDevices.IsEnabled = true;

            base.OnAppearing();
        }
        private void btnSite_Clicked(object sender, EventArgs e)
        {
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("New Site");


            // Reset button Enable in Site Page
            Session.SiteCreateCnt = 0;
            Session.DuctSaveCount = 0;
            Session.RackCount = 0;
            Session.ActiveDeviceCount = 0;

            Navigation.PushAsync(new CreateSite());
        }

        public ICommand ResultCommand { get; set; }

        private async void gotoFOCablePage(object sender, EventArgs e)
        {
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("New Cable");
            await Navigation.PushAsync(new FiberOpticCablePage());
        }

        private void btnPullCable_Clicked(object sender, EventArgs e)
        {
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("Pull Cable");
            Navigation.PushAsync(new PullCablePage());
        }

        private void btnSpliceCable_Clicked(object sender, EventArgs e)
        {
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("Splice Fiber");
            Navigation.PushAsync(new SpliceFiberPage());
        }

        private void btnTerminate_Clicked(object sender, EventArgs e)
        {
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("Terminate Fiber");
            Navigation.PushAsync(new TerminateFiber());
        }

        private void btnInstallDev_Clicked(object sender, EventArgs e)
        {

        }

        private void OTDR_Clicked(object sender, EventArgs e)
        {

        }

        private void btnTraceFiber_Clicked(object sender, EventArgs e)
        {
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("Trace a Fiber");
            Navigation.PushAsync(new TraceFiberMenu());
        }

        private void Install_Clicked(object sender, EventArgs e)
        {

        }
    }
}