using FTCollectorApp.Model;
using FTCollectorApp.ViewModel;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Acr.UserDialogs;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashDownloadPage : ContentPage
    {


        public SplashDownloadPage()
        {
            InitializeComponent();
            BindingContext = new SplashDownloadViewModel();
            Session.Result = "InitializingDownload";

        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine();

            /*if(BindingContext is SplashDownloadViewModel vm)
            {
                if(vm.CheckButtonLogin())
                    Shell.SetNavBarIsVisible(this, true); 
                else
                    Shell.SetNavBarIsVisible(this, false);

            }*/
        }


    }
}