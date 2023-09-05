using System.Web;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using SQLite;
using Xamarin.Forms;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Model;
using System.Collections.ObjectModel;
using FTCollectorApp.View.Utils;
using FTCollectorApp.View.SitesPage.Popup;
using System.Collections.Generic;
using FTCollectorApp.Services;

namespace FTCollectorApp.ViewModel
{
    public partial class PullBoxQuestions2VM : ObservableObject
    {
        [ObservableProperty] string diameter;
        [ObservableProperty] string lidPieces;
        [ObservableProperty] bool isHasAppron = false;
        [ObservableProperty] bool isSpliceVault = false;
        [ObservableProperty] bool isGravelBottom = false;
        [ObservableProperty] bool isHasKey = false;

        [ObservableProperty] string keyType;


        bool isKeyTypeDisplay = false;
        public bool IsKeyTypeDisplay
        {
            get => isKeyTypeDisplay;
            set
            {
                if (IsHasKey)
                    SetProperty(ref isKeyTypeDisplay, true);
                else
                    SetProperty(ref isKeyTypeDisplay, false);
            }
        }
        public ICommand CaptureCommand { get; set; }
        public ICommand CompleteSiteCommand { get; set; }

        public PullBoxQuestions2VM()
        {
            CaptureCommand = new Command(
                execute: async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
                }
            );
            CompleteSiteCommand = new Command(
                execute: async () =>
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new CompleteSitePopUp());
                },
                canExecute: () =>
                {
                    return Session.SiteCreateCnt > 0;
                }
            );
        }


        [ObservableProperty]
        Mounting selectedMounting;

        public ObservableCollection<MaterialCode> MaterialCodeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<MaterialCode>();
                    var table = conn.Table<MaterialCode>().ToList();
                    foreach (var col in table)
                    {
                        col.CodeDescription = HttpUtility.HtmlDecode(col.CodeDescription); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<MaterialCode>(table);
                }
            }
        }
        public ObservableCollection<Mounting> MountingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Mounting>();
                    var mountingTable = conn.Table<Mounting>().OrderBy(a => a.MountingType).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<Mounting>(mountingTable);

                }
            }
        }

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            /*url: 'ajaxSavecabinet.php',      
            data: {"time":getCurtime(),"owner2": owner2,"tag": tag2,"site2": site2,"sitname2": sitname2,
            "mfd2": mfd2,"mfr2": mfr2,"mod2": mod2,"pic2": pic2,"rs2": rs2,"height2": height2,
            "depth2": depth2,"width2": width2,"CLEAR_ZONE_IND2": CLEAR_ZONE_IND2,"longitude2": longitude2,
            "lattitude2": lattitude2,"intersect2": intersect2,"material2": material2,"mounting2": mounting2,
            "offilter2": offilter2,"fltrsize2": fltrsize2,"sunshield2": sunshield2,"installed2": installed2
            ,"comment2": comment2,"etc2": etc2,"fosc2": fosc2,"vault2": vault2,"trlane2": trlane2,
            "bucket2": bucket2,"type2":type2,"serialno":serialno,"ground":ground,"key":key,"ktype":ktype,
            "traveldir":traveldir,"roadway":roadway,"accuracy":accuracy,"altitude":altitude,"loct":loct,"ctype":ctype,
            "laneclosure":laneclosure,"dotdis":dotdis,"powr":powr,"elecsite":elecsite,"comm":comm,"commprovider":commprovider,"sitaddr":sitaddr,"udsowner":udsowner,"orientationid":orientationid,"otag":otag},*/

            Console.WriteLine();
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 2
                new KeyValuePair<string, string>("accuracy", Session.accuracy), //3
                new KeyValuePair<string, string>("altitude", Session.altitude),  //4
                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                //new KeyValuePair<string, string>("owner", Session.ownerkey), //5
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 

                new KeyValuePair<string, string>("tag",Session.tag_number), //8
                //new KeyValuePair<string, string>("site2", SiteName),  /// site_id
                //new KeyValuePair<string, string>("owner_tag_number", OwnerTagNumber),  /// site_id
                new KeyValuePair<string, string>("type2", Session.site_type_key),  /// code_site_type.key

                // pullbox - start
                new KeyValuePair<string, string>("diameter", Diameter),
                new KeyValuePair<string, string>("gravel_bottom",IsGravelBottom ? "1":"0" ),
                new KeyValuePair<string, string>("lid_pieces", LidPieces),

                new KeyValuePair<string, string>("gravel_bottom",IsGravelBottom ? "1" : "0" ),
                new KeyValuePair<string, string>("has_apron", IsHasAppron ? "1" : "0"),
                //new KeyValuePair<string, string>("rack_count", SelectedRackCount is null ? "" : SelectedRackCount),

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("splicevault", IsSpliceVault ? "1":"0"),
                //new KeyValuePair<string, string>("trlane2", DistanceEOTL),
                //new KeyValuePair<string, string>("bucket2", IsBucketTruck ? "1":"0"),
                new KeyValuePair<string, string>("serialno", ""),
                new KeyValuePair<string, string>("haskey", IsHasKey ? "1":"0"),
                new KeyValuePair<string, string>("ktype", IsHasKey ? KeyType : ""), //SelectedKeyType),
                //new KeyValuePair<string, string>("ground", IsHasGroundRod ? "1":"0"),
                //new KeyValuePair<string, string>("traveldir", SelectedTravelDirection?.CompasKey is null ? "": SelectedTravelDirection.CompasKey),
                //new KeyValuePair<string, string>("traveldir2", SelectedTravelDirection2?.CompasKey is null ? "": SelectedTravelDirection2.CompasKey),
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("owner_county", Session.countycode),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),

                new KeyValuePair<string, string>("gps_offset_latitude", ""),
                new KeyValuePair<string, string>("gps_offset_longitude", ""),
                new KeyValuePair<string, string>("LATITUDE", Session.lattitude2),
                new KeyValuePair<string, string>("LONGITUDE", Session.longitude2),


                new KeyValuePair<string, string>("plansheet","0"),
                new KeyValuePair<string, string>("psitem","0"),
                new KeyValuePair<string, string>("stage", Session.stage),
            };


            return keyValues;

        }


        [ICommand]
        async void SaveContinue()
        {
            Console.WriteLine();
            try
            {
                var KVPair = keyvaluepair();
                //var result = await CloudDBService.PostSaveBuilding(KVPair);
                var result = await CloudDBService.UpdateSiteQuestionPage2(KVPair);
                if (result.Equals("OK"))
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Uploading Data Done", "OK");
                    Session.CabinetPage2CreateCnt = 1;
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", result, "RETRY");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.ToString());
                await Application.Current.MainPage.DisplayAlert("Warning", "Data Insert Problem", "BACK");
            }

        }
    }
}