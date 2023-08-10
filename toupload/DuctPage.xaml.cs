using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Collections.ObjectModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.Utils;
using FTCollectorApp.Model;
using FTCollectorApp.Services;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuctPage : ContentPage
    {

        public DuctPage()
        //public DuctPage(string ResultPage)
        {
            InitializeComponent();

            BindingContext = new DuctViewModel();
        }

        

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }

     }
}