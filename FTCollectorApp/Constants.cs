using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp
{

    public class Constants
    {
        public const bool AutoSyncAWSTables = false;

        public const string LiveDBurl = "https://collector.fibertrak.com/FTService/";  //myfibertrak db
        public const string FloridaDB = "https://collector.fibertrak.com/florida/";  //test db
        public const string MariettaDB = "https://collector.fibertrak.com/mariettadb/";  //Marietta db
        public const string GeorgiaDB = "https://collector.fibertrak.com/Georgiadb/";  //Georgia db
        public const string TestingDB = "https://collector.fibertrak.com/TestingDB/";  //Testing db
        

        public static string BaseUrl = TestingDB;
        
        public const string InsertTimeSheetUrl = "InsertTimeSheet.php";
        public const string InsertJobEquipmentUrl = "xPostJobEquipment.php";
        public const string GetJobTableUrl = "xamarinJob.php";
        public const string GetEndUserTableUrl = "xamarinLogin.php";
        public const string InsertJobEvents = "InsertJobEvents.php"; // xSaveJobEvents.php";
        public const string GetSiteTableUrl= "xGetSite.php";
        public const string GetCrewdefaultTableUrl = "getCrewdefault.php";
        public const string GetCodeSiteTypeTableUrl = "xGetCodeSiteType.php";
        public const string SaveCrewUrl = "saveCrew.php";
        public const string CreateSiteTableUrl = "Createsite.php";
        public const string UpdateSiteTableUrl = "Savebuilding.php";
        public const string UpdateAfiberCableTableUrl = "ajaxSavefosccable.php";// ajaxSavefosc.php";
        public const string ajaxSaveduct = "ajaxSaveduct.php";
        public const string GetBuildingsParamUrl = "getBuildingEntries.php";
        public const string PostSplice = "ajaxSavesplice.php";
        public const string SaveRacks = "ajaxSaverack.php";
        public const string SaveActiveDevice = "ajaxSaveactivedevice.php";
        public const string SaveSlack = "ajaxSaveslack.php";
        public const string SaveSheathMark = "ajaxSaveSheathmark.php";
        public const string ajaxSaveSlotBTray = "ajaxSaveblade.php";

        public const string ajaxSavePorts = "ajaxSavePorts.php";
        public const string ajaxSavePort = "ajaxSavePortConnection.php";
        public const string ajaxUpdateIpAddr = "ajaxUpdateipaddr.php";

        public const string ajaxSavePullFiber = "ajaxSavepullfiber.php";
        public const string ajaxSavepicturename = "ajaxSavepicturename.php";
        public const string ajaxSaveTraceWire = "ajaxSaveTraceWire.php";
        public const string ajaxSaveDuctTrace = "ajaxSaveDuctTrace.php";
        public const string ajaxSaveEndDuctTrace = "ajaxSaveEndDuctTrace.php";

        public const string ajaxEquipmentCheckIn = "ajaxEquipmentcheckin.php";
        public const string ajaxSignature = "saveImagee.php";

        public const string ajaxSaveGPSPoint = "ajaxSaveGPSPoint.php";
        public const string UpdateSite = "Updatesite.php";

        public static string TaskCount = "task_count";
        public static string SavedFromDuctTagNumber = "from_duct_tag";
        public static string SavedFromDuctTagNumberKey = "from_duct_tag_key";
        public static string SavedToDuctTagNumber = "to_duct_tag";
        public static string SavedToDuctTagNumberKey = "to_duct_tag_key";
        public static string LastSavedBeginningDuctKey = "duct_in_duct_trace";



        //////////////////// AWS S3 params - start ////////////////////////////////
        public const string COGNITO_POOL_ID = "us-east-2:5ad27ed4-59be-49f6-b103-3edb3e4d20c5";

        /*
        * Region of your Cognito identity pool ID.
        */
        public const string COGNITO_POOL_REGION = "us-east-2";

        /*
            * Note, you must first create a bucket using the S3 console before running
            * the sample (https://console.aws.amazon.com/s3/). After creating a bucket,
            * put it's name in the field below.
            */
        public const string BUCKET_NAME = "fibertrakmedia";

        /*
            * Region of your bucket.
            */
        public const string BUCKET_REGION = "us-east-2";
        public const string ACCES_KEY_ID = "AKIAJTM6EJOVYMZEVPPQ";
        public const string SECRET_ACCESS_KEY = "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";
        //////////////////// AWS S3 params - end ////////////////////////////////
        ///


        public const string ACTION_START_SERVICE = "FTCollectorApp.action.START_SERVICE";
        public const string ACTION_STOP_SERVICE =  "FTCollectorApp.action.STOP_SERVICE";
        public const string ACTION_RESTART_TIMER = "FTCollectorApp.action.RESTART_TIMER";
        public const string ACTION_MAIN_ACTIVITY = "FTCollectorApp.action.MAIN_ACTIVITY";
    }
}
