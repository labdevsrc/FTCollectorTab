using FTCollectorApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using Plugin.Connectivity;
using FTCollectorApp.View;
using FTCollectorApp.View.SitesPage;

using FTCollectorApp.Services;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.Utils;
using FTCollectorApp.ViewModel;
using FontAwesome;
//using Acr.UserDialogs;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyJobPage : ContentPage
    {

        public List<string> OwnerName;
        private const int PageNumber = 3;
        private const string PageNumberKey = "PageNumber";
        private const string JobOwnerKey = "JobOwner";
        private const string JobNumberKey = "JobNumber";
        private ObservableCollection<Job> _jobdetails = new ObservableCollection<Job>();

        public VerifyJobPage()
        {
            InitializeComponent();

            BindingContext = new VerifyJobViewModel();

            MessagingCenter.Subscribe<VerifyJobViewModel>(this, "OpenSitePage", (sender) =>
            {

                Console.WriteLine();

                ShellSection shell_section = new ShellSection
                {
                    Title = "SITE",
                    Icon = "building.png"
                };

                shell_section.Items.Add(new ShellContent() {
                    //Content = new MainSitePageX(),
                    Content = new CreateSitewQuestion1(),

                });
                AppShell.mytabbar.Items.Add(shell_section);


                Console.WriteLine();
            });
        }


        private void btnFindMe(object sender, EventArgs e)
        {


            var browser = new WebView
            {
                Source = "http://ec2-52-14-97-126.us-east-2.compute.amazonaws.com/FiberTrakArcGIS/UserMap.aspx?user=" + Session.uid
            };
        }


    }
}