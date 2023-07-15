﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

using FTCollectorApp.View;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.ViewPortrait;
using FTCollectorApp.View.FormStyle;
using FTCollectorApp.View.CablePages;
using FTCollectorApp.View.TraceFiberPages;

namespace FTCollectorApp
{
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
        public ICommand HelpCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public static TabBar mytabbar;
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;
            mytabbar = AllTab;

        }


        void RegisterRoutes()
        {

            try
            {
                Routes.Add(nameof(VerifyJobPage), typeof(VerifyJobPage));

                Routes.Add(nameof(BdSiteNew), typeof(BdSiteNew)); // Portrait, 3 columns of BdSite
                Routes.Add(nameof(CabinetSitePageView), typeof(CabinetSitePageView));
                Routes.Add(nameof(PullBoxSitePageView), typeof(PullBoxSitePageView));
                Routes.Add(nameof(StructureSitePageView), typeof(StructureSitePageView));
                Routes.Add(nameof(DuctPage), typeof(DuctPage));
                Routes.Add(nameof(ActiveDevicePage), typeof(ActiveDevicePage));

                Routes.Add(nameof(RacksPage), typeof(RacksPage));
                Routes.Add(nameof(PortPage), typeof(PortPage));
                Routes.Add(nameof(PullCablePage), typeof(PullCablePage));

                Routes.Add(nameof(FiberOpticCablePage), typeof(FiberOpticCablePage));
                Routes.Add(nameof(FiberInstallTabPage), typeof(FiberInstallTabPage));
                Routes.Add(nameof(TabbedSitePage), typeof(TabbedSitePage));

                Routes.Add(nameof(MainPageP), typeof(MainPageP)); //Portrait of SplashDownloadPage, Tab name : MainPAge
                //Routes.Add(nameof(MainSitePageX), typeof(MainSitePageX)); obsolete - nolonger use



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