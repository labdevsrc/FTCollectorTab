using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
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
using FTCollectorApp.View.Utils;
using System.Web;
using FTCollectorApp.Service;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FTCollectorApp.View.SitesPage
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveDevicePage : ContentPage
    {


        public ActiveDevicePage()
        {
            InitializeComponent();
            BindingContext = new ActiveDeviceViewModel();

        }




        /*protected override async void OnAppearing()
        {

            base.OnAppearing();
            txtHostTagNumber.Text = Session.tag_number;
        }

        string InstalledAt, Manufactured;
        string SerialNum, selectedWebUrl;


        //bool displayed = false;
        private void SeePic(object sender, EventArgs e)
        {
            //webview.IsVisible = displayed;
            //webview.Source = SelectedModelDetail.PictUrl;
            //displayed = !displayed;

        }

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }*/

    }
}