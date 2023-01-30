using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

using FTCollectorApp.View;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.ViewPortrait;

namespace FTCollectorApp
{
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
        public ICommand HelpCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;

            //if (Shell.Current. == nameof(CabinetSitePageView))
            //    return;

        }

        void RegisterRoutes()
        {

            try
            {
                //Routes.Add(nameof(LoginPage), typeof(LoginPage));
                Routes.Add(nameof(BeginWorkPage), typeof(BeginWorkPage));
                Routes.Add(nameof(VerifyJobPage), typeof(VerifyJobPage));
                //Routes.Add(nameof(SelectCrewPage), typeof(SelectCrewPage));
                //Routes.Add(nameof(AsBuiltDocMenu), typeof(AsBuiltDocMenu));
                //Routes.Add(nameof(MainMenuInstall), typeof(MainMenuInstall));
                Routes.Add(nameof(MainSitePage), typeof(MainSitePage));
               
                //Routes.Add(nameof(CreateSite), typeof(CreateSite));
                
                Routes.Add(nameof(BdSiteNew), typeof(BdSiteNew)); // Portrait, 3 columns of BdSite
                Routes.Add(nameof(CabinetSitePageView), typeof(CabinetSitePageView));
                Routes.Add(nameof(PullBoxSitePageView), typeof(PullBoxSitePageView));
                Routes.Add(nameof(StructureSitePageView), typeof(StructureSitePageView));
                Routes.Add(nameof(DuctPage), typeof(DuctPage));
                Routes.Add(nameof(ActiveDevicePage), typeof(ActiveDevicePage));

                Routes.Add(nameof(RacksPage), typeof(RacksPage));
                Routes.Add(nameof(PortPage), typeof(PortPage));


                //Routes.Add(nameof(SplashDownloadPage), typeof(SplashDownloadPage));
                Routes.Add(nameof(MainPageP), typeof(MainPageP)); //Portrait of SplashDownloadPage, Tab name : MainPAge


                foreach (var item in Routes)
                {
                    Routing.RegisterRoute(item.Key, item.Value);
                }
            }
            catch(Exception e){

                Console.WriteLine("Exception");
            
            }
        }


        //protected override void OnNavigating(ShellNavigatingEventArgs args)
        //{
        //    base.OnNavigating(args);

        //    // Cancel any back navigation
        //    //if (e.Source == ShellNavigationSource.Pop)
        //    //{
        //    //    e.Cancel();
        //    //}
        //}

        //protected override void OnNavigated(ShellNavigatedEventArgs args)
        //{
        //    base.OnNavigated(args);

        //    // Perform required logic
        //}
    }
}