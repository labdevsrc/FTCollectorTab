using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

using AndroidX.Core.App;

using FTCollectorApp.Model;
using FTCollectorApp.Model.AWS;
using FTCollectorApp.Model.Reference;

using FTCollectorApp.Droid.Services;
using FTCollectorApp.Services;
using FTCollectorApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ForegroundService))]

namespace FTCollectorApp.Droid.Services
{
    [Service]
    public class ForegroundService : Service, IForegroundService
    {
        public static bool isForegroundServiceRunning;
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Task.Run(async () =>
            {
                Console.WriteLine("Test Foreground Service");

                try
                {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    MessagingCenter.Send<IForegroundService>(this, "DOWNLOAD_START");
                    //}
                    //);
                    await DownloadTables();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            });


            /*Task.Delay(1000).ContinueWith(t =>
            {
                Notification.Builder notificationBuilder = new Notification.Builder(this)
                    .SetSmallIcon(Resource.Drawable.building)
                    .SetContentTitle("Count Down Active!")
                    .SetContentText(LoadingText);

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.Notify(1000, notificationBuilder.Build());
                //DispatchNotificationThatServiceIsRunning();//This is for repeate every 1s.
            }, TaskScheduler.FromCurrentSynchronizationContext());*/

            string channelID = "ForeGroundServiceChannel";
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                var notificationChannel = new NotificationChannel(channelID, channelID, NotificationImportance.Low);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

            var notificationBuilder = new NotificationCompat.Builder(this, channelID)
                                         .SetContentTitle("Field Data Collection")
                                         .SetSmallIcon(Resource.Mipmap.icon)
                                         .SetContentText("DB Downloading start")
                                         .SetPriority(1)
                                         .SetOngoing(true)
                                         .SetChannelId(channelID)
                                         .SetAutoCancel(true);


            StartForeground(1001, notificationBuilder.Build());
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnCreate()
        {
            base.OnCreate();
            isForegroundServiceRunning = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Console.WriteLine();
            isForegroundServiceRunning = false;
        }

        public void StartMyForegroundService()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(ForegroundService));
            Android.App.Application.Context.StartForegroundService(intent);

        }

        public void StopMyForegroundService()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(ForegroundService));
            Android.App.Application.Context.StopService(intent); // StartForegroundService(intent);

        }

        public bool IsForeGroundServiceRunning()
        {
            return isForegroundServiceRunning;
        }

        string LoadingText;

        async Task DownloadTables()
        {


            LoadingText = "Downloading...";
            try
            {

                LoadingText = "Downloading end_user table";
                var contentUser = await CloudDBService.GetEndUserFromAWSMySQLTable();

                LoadingText = "Downloading Site table";
                var contentSite = await CloudDBService.GetSite();


                LoadingText = "Downloading Job table";
                var contentJob = await CloudDBService.GetJobFromAWSMySQLTable();
                LoadingText = "Downloading Site code";
                var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();

                LoadingText = "Downloading Job Phase table";
                var contentJobPhase = await CloudDBService.GetJobPhasesDetail();


                LoadingText = "Downloading Crew table";
                //var contentSite = await CloudDBService.GetSite();
                //var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();

                var contentCrewDefault = await CloudDBService.GetCrewDefaultFromAWSMySQLTable();

                LoadingText = "Downloading manufacturer table";
                var contentManuf = await CloudDBService.GetManufacturerTable(); //manufacturer_list 
                var contentJobSumittal = await CloudDBService.GetJobSubmittalTable(); //job_submittal
                var contentKeyType = await CloudDBService.GetKeyTypeTable(); // keytype
                var equipmentType = await CloudDBService.GetEquipmentType(); //code_equipment_type
                var equipmentDetail = await CloudDBService.GetEquipmentDetail(); //equipment


                var unitOfmeasure = await CloudDBService.GetUOM(); //unit_of_measurement
                LoadingText = "Downloading code_material table";
                var contentMaterialCode = await CloudDBService.GetMaterialCodeTable(); // material
                var contentMounting = await CloudDBService.GetMountingTable(); // mounting

                LoadingText = "Downloading roadway & intersection table";
                var contentRoadway = await CloudDBService.GetRoadway();  // roadway
                var contentIntersection = await CloudDBService.GetIntersection(); //electric

                //var contentOwnRoadway = await CloudDBService.GetOwnerRoadway();  // owner_roadway, this will be joined to roadway
                var contentElectCircuit = await CloudDBService.GetElectricCircuit(); //intersection
                LoadingText = "Downloading intersection table";

                var contentDirection = await CloudDBService.GetDirection(); //direction
                LoadingText = "Downloading code_duct_size table";
                var contentDuctSize = await CloudDBService.GetDuctSize(); //dsize
                var contentDuctType = await CloudDBService.GetDuctType(); //ducttype
                var contentGroupType = await CloudDBService.GetGroupType(); //grouptype


                var contentDevType = await CloudDBService.GetDevType(); //devtype
                var contentModelDetail = await CloudDBService.GetModelDetail(); //model
                var contentRackNumber = await CloudDBService.GetRackNumber();
                var contentRackType = await CloudDBService.GetRackType(); //racktype
                LoadingText = "Downloading code_fiber_sheath_type table";
                var contentSheath = await CloudDBService.GetSheath(); // sheath
                var contentReelId = await CloudDBService.GetReelId(); // reelid
                var contentOrientation = await CloudDBService.GetOrientation();  // sbto
                var contentChassis = await CloudDBService.GetChassis();  // sbto

                LoadingText = "Downloading a_fiber_cable table";
                var contentAFCable = await CloudDBService.GetAFCable();  // frcable
                LoadingText = "Downloading a_fiber_reel table";
                var contentCabStructure = await CloudDBService.GetCableStructure(); //cable_structure

                //var contentSide = await CloudDBService.GetSide(); //side
                var contentTraceWareTag = await CloudDBService.GetTraceWareTag(); // tracewaretag
                LoadingText = "Downloading owner table";
                var contentOwner = await CloudDBService.GetOwners(); //owners
                LoadingText = "Downloading conduits table";
                var contentConduit = await CloudDBService.GetConduits(); // conduits

                LoadingText = "Downloading duct_installtype table";
                var ductInstallType = await CloudDBService.GetDuctInstallType(); // installtype

                LoadingText = "Downloading fiber_installtype table";
                var fiberInstallType = await CloudDBService.GetFiberInstallType(); // installtype

                LoadingText = "Downloading ductused table";
                var contentDuctUsed = await CloudDBService.GetDuctUsed(); // ductused


                LoadingText = "Downloading dimesnsions table";
                var contentDimension = await CloudDBService.GetDimensions();   // dimesnsions

                LoadingText = "Downloading fltrsizes table";
                var contentFilterSize = await CloudDBService.GetFilterSize(); //fltrsizes
                LoadingText = "Downloading  code_filter_type table";
                var contentFilterType = await CloudDBService.GetFilterType(); //fltrsizes
                var contentSpliceType = await CloudDBService.GetSpliceType();//splicetype
                var contentLaborClass = await CloudDBService.GetLaborClass();// laborclass


                var contentTravellen = await CloudDBService.GetCompassDir(); // travellen
                LoadingText = "Downloading building_type table";
                var contentBuildingType = await CloudDBService.GetBuildingType(); //bClassification

                LoadingText = "Downloading code_cable_type table";
                var contentCableType = await CloudDBService.GetCableType(); //code_cable_type

                var codeDuctInstallType = await CloudDBService.GetDuctInstallTypes(); //code_duct_installation

                var codeColor = await CloudDBService.GetColorCode(); //code_colors

                var contentChassisType = await CloudDBService.GetChassisTypes(); //code_colors
                var contentslotBladeTray = await CloudDBService.GetBladeTableKey(); //slotbladetray

                var portType = await CloudDBService.GetCodePortType(); //code_port_type
                var portTable = await CloudDBService.GetPortTable(); //port table


                LoadingText = "Downloading code_locate_point table";

                var codeLocatePoint = await CloudDBService.GetLocatePoint(); //code_locate_point

                var max_gps_point = await CloudDBService.GetMaxGpsPoint(); //gps_point

                var suspList = await CloudDBService.GetSuspendedTrace(); //gps_point
                var excludeSiteEntry = await CloudDBService.GetExcludeSite(); //exclude_site

                var equipCO = await CloudDBService.GetEquipCO(); //equipment_for_checkout

                var getEvent18StartTime = await CloudDBService.GetEvent18Time(); //exclude_site

                //Thread.Sleep(5000);
                LoadingText = "Download done! Populating SQLite...";
                var getCabinetType = await CloudDBService.GetCabinetType();

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    conn.CreateTable<User>();
                    conn.DeleteAll<User>();
                    conn.InsertAll(contentUser);


                    conn.CreateTable<Site>();
                    conn.DeleteAll<Site>();
                    conn.InsertAll(contentSite);


                    conn.CreateTable<Job>();
                    conn.DeleteAll<Job>();
                    conn.InsertAll(contentJob);

                    conn.CreateTable<CodeSiteType>();
                    conn.DeleteAll<CodeSiteType>();
                    conn.InsertAll(contentCodeSiteType);


                    conn.CreateTable<JobPhaseDetail>();
                    conn.DeleteAll<JobPhaseDetail>();
                    conn.InsertAll(contentJobPhase);


                    conn.CreateTable<Crewdefault>();
                    conn.DeleteAll<Crewdefault>();
                    conn.InsertAll(contentCrewDefault);

                    conn.CreateTable<Manufacturer>();
                    conn.DeleteAll<Manufacturer>();
                    conn.InsertAll(contentManuf);

                    conn.CreateTable<JobSubmittal>();
                    conn.DeleteAll<JobSubmittal>();
                    conn.InsertAll(contentJobSumittal);

                    conn.CreateTable<KeyType>();
                    conn.DeleteAll<KeyType>();
                    conn.InsertAll(contentKeyType);

                    conn.CreateTable<EquipmentType>();
                    conn.DeleteAll<EquipmentType>();
                    conn.InsertAll(equipmentType);

                    conn.CreateTable<EquipmentDetailType>();
                    conn.DeleteAll<EquipmentDetailType>();
                    conn.InsertAll(equipmentDetail);

                    conn.CreateTable<MaterialCode>();
                    conn.DeleteAll<MaterialCode>();
                    conn.InsertAll(contentMaterialCode);

                    conn.CreateTable<Mounting>();
                    conn.DeleteAll<Mounting>();
                    conn.InsertAll(contentMounting);

                    conn.CreateTable<Roadway>();
                    conn.DeleteAll<Roadway>();
                    conn.InsertAll(contentRoadway);


                    conn.CreateTable<InterSectionRoad>();
                    conn.DeleteAll<InterSectionRoad>();
                    conn.InsertAll(contentIntersection);


                    conn.CreateTable<ElectricCircuit>();
                    conn.DeleteAll<ElectricCircuit>();
                    conn.InsertAll(contentElectCircuit);

                    conn.CreateTable<Direction>();
                    conn.DeleteAll<Direction>();
                    conn.InsertAll(contentDirection);

                    conn.CreateTable<DuctSize>();
                    conn.DeleteAll<DuctSize>();
                    conn.InsertAll(contentDuctSize);

                    conn.CreateTable<DuctType>();
                    conn.DeleteAll<DuctType>();
                    conn.InsertAll(contentDuctType);

                    conn.CreateTable<GroupType>();
                    conn.DeleteAll<GroupType>();
                    conn.InsertAll(contentGroupType);


                    conn.CreateTable<DevType>();
                    conn.DeleteAll<DevType>();
                    conn.InsertAll(contentDevType);

                    conn.CreateTable<ModelDetail>();
                    conn.DeleteAll<ModelDetail>();
                    conn.InsertAll(contentModelDetail);

                    conn.CreateTable<RackNumber>();
                    conn.DeleteAll<RackNumber>();
                    conn.InsertAll(contentRackNumber);

                    conn.CreateTable<RackType>();
                    conn.DeleteAll<RackType>();
                    conn.InsertAll(contentRackType);

                    conn.CreateTable<Sheath>();
                    conn.DeleteAll<Sheath>();
                    conn.InsertAll(contentSheath);

                    conn.CreateTable<ReelId>();
                    conn.DeleteAll<ReelId>();
                    conn.InsertAll(contentReelId);

                    conn.CreateTable<Chassis>();
                    conn.DeleteAll<Chassis>();
                    conn.InsertAll(contentChassis);

                    conn.CreateTable<UnitOfMeasure>();
                    conn.DeleteAll<UnitOfMeasure>();
                    conn.InsertAll(unitOfmeasure);

                    conn.CreateTable<AFiberCable>();
                    conn.DeleteAll<AFiberCable>();
                    conn.InsertAll(contentAFCable);

                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(ductInstallType);

                    conn.CreateTable<FiberInstallType>();
                    conn.DeleteAll<FiberInstallType>();
                    conn.InsertAll(fiberInstallType);

                    conn.CreateTable<DuctUsed>();
                    conn.DeleteAll<DuctUsed>();
                    conn.InsertAll(contentDuctUsed);



                    /////


                    conn.CreateTable<UnSyncTaskList>();




                    conn.CreateTable<SuspendedTrace>();
                    conn.DeleteAll<SuspendedTrace>();
                    conn.InsertAll(suspList);

                    conn.CreateTable<GpsPoint>();
                    conn.DeleteAll<GpsPoint>();
                    conn.InsertAll(max_gps_point);


                    conn.CreateTable<Owner>();
                    conn.DeleteAll<Owner>();
                    conn.InsertAll(contentOwner);


                    conn.CreateTable<CodeLocatePoint>();
                    conn.DeleteAll<CodeLocatePoint>();
                    conn.InsertAll(codeLocatePoint);

                    conn.CreateTable<PortType>();
                    conn.DeleteAll<PortType>();
                    conn.InsertAll(portType);

                    conn.CreateTable<Ports>();
                    conn.DeleteAll<Ports>();
                    conn.InsertAll(portTable);




                    conn.CreateTable<ChassisType>();
                    conn.DeleteAll<ChassisType>();
                    conn.InsertAll(contentChassisType);

                    conn.CreateTable<CableStructure>();
                    conn.DeleteAll<CableStructure>();
                    conn.InsertAll(contentCabStructure);

                    conn.CreateTable<Dimensions>();
                    conn.DeleteAll<Dimensions>();
                    conn.InsertAll(contentDimension);

                    conn.CreateTable<Orientation>();
                    conn.DeleteAll<Orientation>();
                    conn.InsertAll(contentOrientation);

                    conn.CreateTable<FilterSize>();
                    conn.DeleteAll<FilterSize>();
                    conn.InsertAll(contentFilterSize);

                    conn.CreateTable<FilterType>();
                    conn.DeleteAll<FilterType>();
                    conn.InsertAll(contentFilterType);

                    conn.CreateTable<SpliceType>();
                    conn.DeleteAll<SpliceType>();
                    conn.InsertAll(contentSpliceType);

                    conn.CreateTable<LaborClass>();
                    conn.DeleteAll<LaborClass>();
                    conn.InsertAll(contentLaborClass);

                    conn.CreateTable<CompassDirection>();
                    conn.DeleteAll<CompassDirection>();
                    conn.InsertAll(contentTravellen);

                    conn.CreateTable<BuildingType>();
                    conn.DeleteAll<BuildingType>();
                    conn.InsertAll(contentBuildingType);


                    conn.CreateTable<CableType>();
                    conn.DeleteAll<CableType>();
                    conn.InsertAll(contentCableType);





                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(codeDuctInstallType);



                    conn.CreateTable<ConduitsGroup>();
                    conn.DeleteAll<ConduitsGroup>();
                    conn.InsertAll(contentConduit);


                    conn.CreateTable<ColorCode>();
                    conn.DeleteAll<ColorCode>();
                    conn.InsertAll(codeColor);

                    conn.CreateTable<SlotBladeTray>();
                    conn.DeleteAll<SlotBladeTray>();
                    conn.InsertAll(contentslotBladeTray);


                    conn.CreateTable<ExcludeSite>();
                    conn.DeleteAll<ExcludeSite>();
                    conn.InsertAll(excludeSiteEntry);

                    conn.CreateTable<EquipmentCO>();
                    conn.DeleteAll<EquipmentCO>();
                    conn.InsertAll(equipCO);


                    conn.CreateTable<CrewInfoDetail>();
                    conn.DeleteAll<CrewInfoDetail>();

                    conn.CreateTable<CrewChangeInfoDetail>();
                    conn.DeleteAll<CrewChangeInfoDetail>();
                    conn.InsertAll(getEvent18StartTime);

                    conn.CreateTable<CabinetType>();
                    conn.DeleteAll<CabinetType>();
                    conn.InsertAll(getCabinetType);


                }
                Device.BeginInvokeOnMainThread(() => { MessagingCenter.Send<IForegroundService>(this, "DOWNLOAD_DONE"); });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.ToString());
                //bool answer = await Application.Current.MainPage.DisplayAlert("Warning", "Error during download database", "RETRY", "CLOSE");
                //if (answer)
                //    DownloadTablesT();
                Device.BeginInvokeOnMainThread(() => { MessagingCenter.Send<IForegroundService>(this, "DOWNLOAD_ERROR"); });
                Console.WriteLine(e.ToString());

            }


            LoadingText = "SQLite Dumping done...";


            //IsBusy = false;
            //(LoginCommand as Command).ChangeCanExecute();
            //(DownloadTablesCommand as Command).ChangeCanExecute();
            //(LogoutCommand as Command).ChangeCanExecute();
        }

    }
}