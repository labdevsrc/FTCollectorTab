using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Services;
using FTCollectorApp.View.Utils;
using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Web;
using System.Windows.Input;
using System.ComponentModel;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RacksPage : ContentPage
    {

        Command ResultCommand { get; set; }
        public RacksPage()
        {
            InitializeComponent();
            BindingContext = new RacksViewModel();
        }


        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }


        private async void ExitPage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}