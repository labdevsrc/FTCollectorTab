using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using Android.Content;
using Xamarin.Forms;
//using Acr.UserDialogs;

namespace FTCollectorApp.Droid
{
    [Activity(Label = "FTCollectorApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent startServiceIntent;
        Intent stopServiceIntent;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Maps Initial
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            // SQLite initial
            string dbName = "myfibertrak_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullPath = Path.Combine(folderPath, dbName);

            // signature temp file
            string signature = "signature.png";
            string folderPath_ = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string signaturefullPath= Path.Combine(folderPath_, signature);

            string imgFileName = "sample";// DateTime.Now.ToString("yyyy-MM-dd_HH-MM-SS.png");
            string imgFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string imgFullPath = Path.Combine(imgFolderPath, imgFileName);

            // signature temp file
            string pendingTaskFileName = "pendingFileTask.txt";
            string folderPath__ = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string pendingTaskFileNamefullPath = Path.Combine(folderPath__, pendingTaskFileName);

            Rg.Plugins.Popup.Popup.Init(this);
            //UserDialogs.Init(this);


            //startServiceIntent = new Intent(this, typeof(TimestampService));
            //startServiceIntent.SetAction(Constants.ACTION_START_SERVICE);

            //stopServiceIntent = new Intent(this, typeof(TimestampService));
            //stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);


            LoadApplication(new App(fullPath, signaturefullPath, pendingTaskFileNamefullPath, imgFullPath));


            WireUpLongRunningTask();
        }

        void WireUpLongRunningTask()
        {

            //MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message => {
            //    var intent = new Intent(this, typeof(LongRunningTaskService));
            //});

            //MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
            //    var intent = new Intent(this, typeof(LongRunningTaskService));
            //});
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}