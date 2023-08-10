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
    public partial class CabinetQuestions2 : ContentPage
    {
        /*public BuildingSitePageView(string minorType, string tagNumber)
        {
            InitializeComponent();
            var MajorMinorType = $"Building - {minorType}";
            BindingContext = new BuildingSitePageViewModel(MajorMinorType, tagNumber);
        }*/

        public CabinetQuestions2()
        {
            InitializeComponent();
            BindingContext = new CabinetQuestions2VM();

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
            if (Session.RackCount >= 1)
                btnActiveDevice.IsEnabled = true;
            else if (Session.RackCount == 0)
                btnActiveDevice.IsEnabled = false;


        }


    }
}