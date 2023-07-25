using FTCollectorApp.Model;
using FTCollectorApp.Services;
using FTCollectorApp.View;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp
{
    public partial class App : Application
    {
        public static string DatabaseLocation = string.Empty;
        public static string SignatureFileLocation = string.Empty;
        public static string InternalStorageLocation = string.Empty;
        public static string ImageFileLocation = string.Empty;
        private const string TaskCountKey = "TaskCount";
        private const string PendingTaskKey = "PendingTask";
        /*public App()
        {
            InitializeComponent();


            try
            {
                var test = TaskCount;
            }
            catch
            {
                TaskCount = 0;
            }
            
                
            //MainPage = new NavigationPage(new MainPage()); // root page  is MainPage()
            MainPage = new NavigationPage(new SplashDownloadPage());
        }

        public App(string databaseLoc)
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage());
            //MainPage = new NavigationPage(new SplashDownloadPage());
            MainPage = new AppShell();
            DatabaseLocation = databaseLoc;
        }*/

        public App(string databaseLoc, string signatureLoc, string storeLoc, string imageFilename)
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage());
            //MainPage = new NavigationPage(new SplashDownloadPage());
            MainPage = new AppShell();
            DatabaseLocation = databaseLoc;
            SignatureFileLocation = signatureLoc;
            InternalStorageLocation = storeLoc;
            ImageFileLocation = imageFilename;
        }

        /*protected override void OnStartup()
        {
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "YOUR_API_KEY";
        }*/
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            Console.WriteLine("OnResume()");

        }


        public string PendingTask
        {
            get
            {
                if (Properties.ContainsKey(PendingTaskKey))
                    return Properties[PendingTaskKey].ToString();
                return "";
            }
            set
            {
                Properties[PendingTaskKey] = value;
            }

        }

        public int TaskCount
        {
            get
            {
                if (Properties.ContainsKey(TaskCountKey))
                    return int.Parse(Properties[TaskCountKey].ToString());

                
                return 0;
            }
            set
            {
                Properties[TaskCountKey] = value;
            }
        }




    }
}
