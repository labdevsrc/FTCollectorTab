using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Services;
using FTCollectorApp.View.SitesPage.Popup;
using FTCollectorApp.View.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class StructureQuestions2VM : ObservableObject
    {
        // Question 2

        [ObservableProperty] string streetAddress;
        [ObservableProperty] string postalCode;

        [ObservableProperty] string manufactured;
        [ObservableProperty] string installedAt;

        [ObservableProperty] string selectedElectSiteKey;
        [ObservableProperty] string selectedKeyCode;
        [ObservableProperty] string selectedDistrict;
        [ObservableProperty] string selectedRackCount;

        [ObservableProperty] string commsProvider;
        [ObservableProperty] string serialNumber;
        [ObservableProperty] bool isHasPowerDisconnect = false;
        [ObservableProperty] bool is3rdComms = false;

        [ObservableProperty] bool isHaveSunShield = false;
        [ObservableProperty] string uDSOwner;


        List<KeyValuePair<string, string>> keyvaluepair()
        {

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
                //new KeyValuePair<string, string>("sitname2", SiteName),


                //new KeyValuePair<string, string>("manufactured_date", Manufactured),
                //new KeyValuePair<string, string>("manufacturer", SelectedManuf?.ManufKey is null ? "" : SelectedManuf.ManufKey),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufacturer_key", ""),  // manufacturer , for Cabinet, pull box

                //new KeyValuePair<string, string>("mod2", SelectedModelDetail?.ModelKey is null ? "" : SelectedModelDetail.ModelKey), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                //new KeyValuePair<string, string>("roadway", SelectedRoadway?.RoadwayKey is null ? "" : SelectedRoadway.RoadwayKey),
                new KeyValuePair<string, string>("pid", ""),
                //new KeyValuePair<string, string>("loct", LocationName),

                new KeyValuePair<string, string>("btype", ""),
                //new KeyValuePair<string, string>("orientation", SelectedOrientation?.CompasKey is null ? "" : SelectedOrientation.CompasKey),
                //new KeyValuePair<string, string>("laneclosure", IsLaneClosure ? "1":"0"),
                new KeyValuePair<string, string>("dotdis",  SelectedDistrict is null ? "" : SelectedDistrict),
                new KeyValuePair<string, string>("powr", IsHasPowerDisconnect ? "1":"0"),
                new KeyValuePair<string, string>("elecsite", SelectedElectSiteKey),
                new KeyValuePair<string, string>("comm", Is3rdComms ? "1":"0"),
                new KeyValuePair<string, string>("commprovider", CommsProvider),
                new KeyValuePair<string, string>("sitaddr", StreetAddress), // site_street_addres
                new KeyValuePair<string, string>("udsowner", UDSOwner),

                new KeyValuePair<string, string>("rs2", "L"),

                //new KeyValuePair<string, string>("height2", Height),
                //new KeyValuePair<string, string>("depth2", Depth),
                //new KeyValuePair<string, string>("width2", Width),
                //new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone ? "1":"0"),

                //new KeyValuePair<string, string>("intersect2", SelectedIntersection?.IntersectionKey is null ? "": SelectedIntersection.IntersectionKey),
                //new KeyValuePair<string, string>("material2", SelectedMatCode?.MaterialKey is null ? "":SelectedMatCode.MaterialKey),
                //new KeyValuePair<string, string>("mounting2", SelectedMounting?.MountingKey is null ? "":SelectedMounting.MountingKey),
                //new KeyValuePair<string, string>("offilter2", SelectedFilterType?.FilterTypeKey is null ? "": SelectedFilterType.FilterTypeKey ),//FilterTypeSelected),
                //new KeyValuePair<string, string>("fltrsize2", SelectedFilterSize?.FtSizeKey  is null ? "": SelectedFilterSize.FtSizeKey  ),//FilterSizeKeySelected),
                new KeyValuePair<string, string>("sunshield2", IsHaveSunShield ? "1":"0"),
                new KeyValuePair<string, string>("installed2", InstalledAt),
                //new KeyValuePair<string, string>("comment2", Notes), // Notes, pr description

                new KeyValuePair<string, string>("gravel_bottom","" ),
                new KeyValuePair<string, string>("lid_pieces", ""),
                new KeyValuePair<string, string>("has_apron", ""),
                //new KeyValuePair<string, string>("rack_count", SelectedRackCount is null ? "" : SelectedRackCount),

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                //new KeyValuePair<string, string>("trlane2", DistanceEOTL),
                //new KeyValuePair<string, string>("bucket2", IsBucketTruck ? "1":"0"),
                new KeyValuePair<string, string>("serialno", SerialNumber),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", ""), //SelectedKeyType),
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
                    Session.SiteCreateCnt++;
                    //(ShowDuctPageCommand as Command).ChangeCanExecute();
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

        [ICommand]
        async void CompleteSite()
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new CompleteSitePage());

            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new CompleteSitePopUp());
        }

        [ICommand]
        async void Capture()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }
    }
}
