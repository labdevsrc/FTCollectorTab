using FTCollectorApp.Model;
using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CabinetSitePageView : ContentPage
    {
        //public CabinetSitePageView(string minorType, string tagNumber)
        public CabinetSitePageView()
        {
            InitializeComponent();
            //var MajorMinorType = $"Cabinet - {minorType}";
            //BindingContext = new CabinetSitePageViewModel(MajorMinorType, tagNumber);
            BindingContext = new CabinetSitePageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            //if duct created > 1, enable Rack button in Building Site Pagge
            if (Session.DuctSaveCount >= 1)
            {
                btnRecordRack.IsEnabled = true;
                btnFiberBtn.IsEnabled = true;
            }
            else if (Session.DuctSaveCount == 0)
            {
                btnRecordRack.IsEnabled = false;
                btnFiberBtn.IsEnabled = false;
            }


            //if reacks created > 1, enable Active device  button in Building Site Pagge
            if (Session.RackCount > 1)
                btnActiveDevice.IsEnabled = true;
            else if (Session.RackCount == 0)
                btnActiveDevice.IsEnabled = false;


        }
    }
}