/////////
/// WARNING !  This code-behind MainPage.xaml.cs is obsolete
/// Already replace with LoginViewModel.cs
///////////////////////


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Xamarin.Essentials;
using FTCollectorApp.Model;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using FTCollectorApp.Services;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.View.TraceFiberPages;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.FiberPages;
//using Rg.Plugins.Popup.Services;

namespace FTCollectorApp.View
{

    public partial class MainPage : ContentPage
    {

        // Rajib API variables
        //private HttpClient httpClient = new HttpClient();
        private ObservableCollection<User> Users;

        public MainPage()
        {
            InitializeComponent();
            //BindingContext = new MainPageViewModel();
            Users = new ObservableCollection<User>();
        }


        /*protected override async void OnAppearing()
        {
            Console.WriteLine("Connectivity : " + Connectivity.NetworkAccess);

            // https://stackoverflow.com/questions/40458842/internet-connectivity-listener-in-xamarin-forms
            // https://www.youtube.com/watch?v=aA-sA0ACum0
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            Users.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<User>();

                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    //await GetEndUserFromAWSMySQLTable();

                    var users = await CloudDBService.GetEndUserFromAWSMySQLTable();
                    conn.InsertAll(users);
                    Users = new ObservableCollection<User>(users);

                }
                else
                {
                    var users = conn.Table<User>().ToList();
                    Users = new ObservableCollection<User>(users);
                }
            }
            base.OnAppearing();
        }*/


        protected override async void OnAppearing()
        {
            Console.WriteLine("Connectivity : " + Connectivity.NetworkAccess);

            // https://stackoverflow.com/questions/40458842/internet-connectivity-listener-in-xamarin-forms
            // https://www.youtube.com/watch?v=aA-sA0ACum0
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                //await GetEndUserFromAWSMySQLTable();
                Users.Clear();
                var users = await CloudDBService.GetEndUserFromAWSMySQLTable();

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    //Type classname = object1.GetType();

                    conn.CreateTable<User>();
                    Console.WriteLine("CreateTable<User> ");
                    conn.InsertAll(users);
                }

                Users = new ObservableCollection<User>(users);
            }
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    Console.WriteLine("CreateTable<User> ");
                    var userdetails = conn.Table<User>().ToList();
                    Users = new ObservableCollection<User>(userdetails);
                }
            }



            base.OnAppearing();
            //await LocationService.GetLocation();

        }


        private async Task GetEndUserFromAWSMySQLTable()
        {
            Users.Clear();

            // grab End User tables from Url https://collector.fibertrak.com/phonev4/xamarinLogin.php
            var users = await CloudDBService.GetEndUserFromAWSMySQLTable();
            Users = new ObservableCollection<User>(users);
            Console.WriteLine(users);

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<User>();
                conn.InsertAll(users);
            }
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await GetEndUserFromAWSMySQLTable();
            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            Session.uid = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.UserKey).First(); // populate uid to Static-class (session) property uid  
            Session.crew_leader = $"{txtFirstName.Text} {txtLastName.Text}";                                              //location = LocateService.Coords;
            await Navigation.PushAsync(new VerifyJobPage()); // VerifyJobPage
            //await Navigation.PushAsync(new PortConnection()); // VerifyJobPage
            //await Navigation.PushAsync(new EqCheckOutPage()); // VerifyJobPage
            //await Navigation.PushAsync(new DuctTracePage()); // VerifyJobPage
            //await Navigation.PushAsync(new RacksPage()); // VerifyJobPage
            //await Navigation.PushAsync(new ActiveDevicePage()); // VerifyJobPage
            //await Navigation.PushAsync(new TerminateFiber());
            //await Navigation.PushAsync(new SlotBladePage());
        }


        private void entryEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtFirstName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.first_name).First();
                txtLastName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.last_name).First();
                Console.WriteLine(txtFirstName.Text + " " + txtLastName.Text);
            }
            catch (Exception exception)
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";
                Console.WriteLine(exception.ToString());
            }
        }

        private void entryPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtFirstName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.first_name).First();
                txtLastName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.last_name).First();
                Console.WriteLine(txtFirstName.Text + " " + txtLastName.Text);
            }
            catch (Exception exception)
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";

                Console.WriteLine(exception.ToString());
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

    }
}
