using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using FTCollectorApp.Services;

namespace FTCollectorApp.ViewModel
{
    public partial class PullCableViewModel : ObservableObject
    {

        [ObservableProperty] bool newFiber = false;
        [ObservableProperty] FiberInstallType selectedInstallType;
        [ObservableProperty] Site selectedSiteOut;
        [ObservableProperty] Site selectedSiteIn;
        [ObservableProperty] ConduitsGroup selectedDuctOut;
        [ObservableProperty] ConduitsGroup selectedDuctIn;
        [ObservableProperty] string sheathOutNumber = string.Empty;
        [ObservableProperty] string sheathInNumber = string.Empty;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CableIdByTypeList))]
        AFiberCable selectedFiberCable;


        [ObservableProperty] AFiberCable selectedCableId;
        public ObservableCollection<CableType> CableTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CableType>();
                    var table = conn.Table<CableType>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<CableType>(table);
                }
            }
        }


        public ObservableCollection<AFiberCable> CableIdByTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    conn.CreateTable<AFiberCable>();
                    var afiber = conn.Table<AFiberCable>();
                    try
                    {
                        if (SelectedCableType != null)
                        {
                            var tmp = afiber.Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum && a.CableType == SelectedCableType.CodeCableKey).ToList();
                            //var table = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum).ToList();
                            Console.WriteLine();
                            foreach (var col in tmp)
                            {
                                col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                            }
                            return new ObservableCollection<AFiberCable>(tmp);
                        }
                        return new ObservableCollection<AFiberCable>(afiber.ToList());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return new ObservableCollection<AFiberCable>();
                    }
                }
            }
        }

        public ObservableCollection<Sheath> SheathList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Sheath>();
                    Console.WriteLine();
                    var table = conn.Table<Sheath>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<Sheath>(table);
                }
            }
        }

        public ObservableCollection<FiberInstallType> InstallTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FiberInstallType>();
                    Console.WriteLine();
                    var table = conn.Table<FiberInstallType>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<FiberInstallType>(table);
                }
            }
        }


        public ObservableCollection<Site> SiteInOutList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    Console.WriteLine();
                    var table = conn.Table<Site>().Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum).ToList();
                    foreach (var col in table)
                    {
                        if (col.SiteName == null)
                            col.SiteName = col.TagNumber;
                    }

                    Console.WriteLine();
                    return new ObservableCollection<Site>(table);
                }
            }
        }



        public ObservableCollection<ConduitsGroup> DuctInOutList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ConduitsGroup>();
                    Console.WriteLine();
                    try
                    {
                        string CreatorID = Session.uid.ToString();
                        var table = conn.Table<ConduitsGroup>().Where(a => a.OwnerKey == Session.ownerkey
                        && a.CreatedBy == CreatorID).GroupBy(a => a.HosTagNumber).First().ToList();
                        Console.WriteLine();

                        if (SelectedDuctIn?.HosTagNumber != null)
                            table = conn.Table<ConduitsGroup>().Where(a => a.HosTagNumber
                            == SelectedDuctIn.HosTagNumber).ToList();
                        return new ObservableCollection<ConduitsGroup>(table);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    return new ObservableCollection<ConduitsGroup>();
                }
            }
        }



        [ObservableProperty] string loadingText = "";
        [ObservableProperty] bool isBusy = false;
        [ObservableProperty] CableType selectedCableType;

        async void SyncAWSDB()
        {
            LoadingText = "Downloading a_fiber_cable table";
            IsBusy = true;
            var contentAFCable = await CloudDBService.GetAFCable();  // get a_fiber_Cable update

            // upload to SQLite local
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<AFiberCable>();
                conn.DeleteAll<AFiberCable>();
                conn.InsertAll(contentAFCable);
            }
            IsBusy = false;

        }

        public PullCableViewModel()
        {
            Console.WriteLine();
            try
            {
                SyncAWSDB();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }



        [ICommand]
        async void SavePullFiber()
        {
            Console.WriteLine();
            var KVPair = keyvaluepair();
            Console.WriteLine();
            string result = await CloudDBService.PostSavePullFiber(KVPair);
            if (result.Equals("OK"))
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Uploading Data Done", "DONE");

                /*if (choice)
                {
                    if (Session.stage.Equals("A"))
                        await Application.Current.MainPage.Navigation.PushAsync(new AsBuiltDocMenu());
                    if (Session.stage.Equals("I"))
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenuInstall());
                }*/
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fail", "Uploading Data Fail. Check internet Connection", "RETRY", "DONE");
            }
        }
        List<KeyValuePair<string, string>> keyvaluepair()
        {

            string CableName = string.Empty;
            if (SelectedCableId?.CableIdDesc is null)
                CableName = "NA";
            else
                CableName = SelectedCableId.CableIdDesc;

            var keyValues = new List<KeyValuePair<string, string>>{
               new KeyValuePair<string, string>("cable_id_key", SelectedFiberCable.AFRKey),
                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // #1
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobnum", Session.jobkey),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("jobnum", Session.jobnum), //  7 
                new KeyValuePair<string, string>("stage", Session.stage),

                new KeyValuePair<string, string>("cable_id_key", SelectedFiberCable.AFRKey),
                new KeyValuePair<string, string>("to_duct", SelectedDuctOut.ConduitKey),
                new KeyValuePair<string, string>("from_duct", SelectedDuctIn.ConduitKey),
                new KeyValuePair<string, string>("to_duct_dir", SelectedDuctOut.ConduitKey),
                new KeyValuePair<string, string>("from_duct_dir", SelectedDuctIn.ConduitKey),
                new KeyValuePair<string, string>("to_duct_dir_cnt", SelectedDuctOut.ConduitKey),
                new KeyValuePair<string, string>("from_duct_dir_cnt", SelectedDuctIn.ConduitKey),

                new KeyValuePair<string, string>("to_tagnumber", SelectedSiteIn.TagNumber),
                new KeyValuePair<string, string>("from_tagnumber", SelectedSiteOut.TagNumber),
                new KeyValuePair<string, string>("cabletype", SelectedCableType.CodeCableKey),
                new KeyValuePair<string, string>("install", SelectedInstallType.FbrInstallKey),
                new KeyValuePair<string, string>("sheathout", SheathOutNumber),
                new KeyValuePair<string, string>("sheathin", SheathInNumber),
            };
            return keyValues;
        }


    }
}
