using FTCollectorApp.View.TraceFiberPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage.Fiber
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiberMainMenu : ContentPage
    {
        public FiberMainMenu()
        {
            InitializeComponent();
        }

        private async void btnCable_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FiberOpticCablePage());
        }

        private async void btnSheathMark_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SheathMark());
        }

        private void btnSlack_Clicked(object sender, EventArgs e)
        {

        }

        private void btnTraceFiber_Clicked(object sender, EventArgs e)
        {

        }

        private void btnEnclosure_Clicked(object sender, EventArgs e)
        {

        }

        private void btnSaveContinue_Clicked(object sender, EventArgs e)
        {

        }
    }
}