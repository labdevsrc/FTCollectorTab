using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.SitesPage;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public class DropDownViewModel : BaseViewModel
    {
        // Racks Page - start
        public DropDownViewModel()
        {
            ShowDuctPageCommand = new Command(async () => ExecuteNavigateToDuctPageCommand());
            ShowRackPageCommand = new Command(async () => ExecuteNavigateToRackPageCommand());
            ShowActiveDevicePageCommand = new Command(async () => ExecuteNavigateToActiveDevicePageCommand());
            SendResultCommand = new Command(resultPage => ExecuteGetResultCommand(ResultPage));

        }
        ////////////
        ///
        public ICommand SendResultCommand { get; }
        public ICommand ShowDuctPageCommand { get; }
        private async Task ExecuteNavigateToDuctPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new DuctPage());
        }

        public ICommand ShowRackPageCommand { get; }
        private async Task ExecuteNavigateToRackPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RacksPage());
        }

        public ICommand ShowActiveDevicePageCommand { get; }
        private async Task ExecuteNavigateToActiveDevicePageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ActiveDevicePage());
        }


        //public string ResultPage;
        private string _resultPage;
        public string ResultPage
        {
            get => _resultPage;
            set
            {
                _resultPage = value;
                OnPropertyChanged(nameof(ResultPage));
            }
        }

        private void ExecuteGetResultCommand(string result)
        {
            ResultPage = result;
            Console.WriteLine();
        }





        public ObservableCollection<RackType> RackTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackType>();
                    var table = conn.Table<RackType>().ToList();
                    return new ObservableCollection<RackType>(table);
                }
            }
        }

        // Racks Page - end


        // Splice Fiber (SpliceFiberPage) - start
        public ObservableCollection<Site> SitebyJobOwnerCreated
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    var ownerid = Session.uid.ToString();
                    var table = conn.Table<Site>().ToList();
                    var table2 = table.
                        Where(a => (a.JobKey == Session.jobkey) && (a.OWNER_CD == Session.ownerCD)).OrderBy(a => a.TagNumber);
                    Console.WriteLine();
                    return new ObservableCollection<Site>(table2);
                }
            }
        }

        // Splice Fiber (SpliceFiberPage) - end



        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<EquipmentType>();
                    var table = conn.Table<EquipmentType>().ToList();
                    return new ObservableCollection<EquipmentType>(table);
                }
            }
        }


        public ObservableCollection<EquipmentDetailType> EquipmentDetailTypes
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<EquipmentDetailType>();
                    var table = conn.Table<EquipmentDetailType>().ToList();
                    return new ObservableCollection<EquipmentDetailType>(table);
                }
            }
        }
        /// <summary>
        /// Site Input Page Drop down val
        /// </summary>
        public ObservableCollection<CodeSiteType> StructureSiteType
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    var CodeSiteTable = conn.Table<CodeSiteType>().Where(a => a.MajorType == "Structure").ToList();
                    return new ObservableCollection<CodeSiteType>(CodeSiteTable);
                }
            }
        }
        public ObservableCollection<CodeSiteType> CodeSite
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    var table = conn.Table<CodeSiteType>().ToList();
                    return new ObservableCollection<CodeSiteType>(table);
                }
            }
        }

        public ObservableCollection<BuildingType> BuildingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<BuildingType>();
                    var bdClassiTable = conn.Table<BuildingType>().ToList();
                    return new ObservableCollection<BuildingType>(bdClassiTable);
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
                    var mountingTable = conn.Table<Mounting>().ToList();
                    return new ObservableCollection<Mounting>(mountingTable);
                }
            }
        }
        public ObservableCollection<InterSectionRoad> IntersectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<InterSectionRoad>();
                    var data = conn.Table<InterSectionRoad>().Where(a => a.OWNER_CD == Session.ownerCD).GroupBy(b => b.IntersectionName).Select(g => g.First()).ToList();
                    return new ObservableCollection<InterSectionRoad>(data);
                }
            }
        }


        public ObservableCollection<Roadway> RoadwayList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Roadway>();
                    var rwTable = conn.Table<Roadway>().ToList();
                    var table = rwTable.Where(a => a.RoadOwnerKey == Session.ownerkey).ToList();
                    return new ObservableCollection<Roadway>(table);
                }
            }
        }

        /// Building Site Page, Structure Site Page, 
        /// 

        public ObservableCollection<CompassDirection> TravelDirectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CompassDirection>();
                    var data = conn.Table<CompassDirection>().ToList();
                    return new ObservableCollection<CompassDirection>(data);
                }
            }
        }

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
                    return new ObservableCollection<MaterialCode>(table);
                }
            }
        }


        public ObservableCollection<FilterType> FilterTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FilterType>();
                    var table = conn.Table<FilterType>().ToList();
                    foreach (var col in table)
                    {
                        col.FilterTypeDesc = HttpUtility.HtmlDecode(col.FilterTypeDesc); // should use for escape char "
                    }
                    return new ObservableCollection<FilterType>(table);
                }
            }
        }

        public ObservableCollection<FilterSize> FilterSizeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FilterSize>();
                    var table = conn.Table<FilterSize>().ToList();
                    foreach (var col in table)
                    {
                        col.data = HttpUtility.HtmlDecode(col.data); // should use for escape char "
                    }
                    return new ObservableCollection<FilterSize>(table);
                }
            }
        }

        public ObservableCollection<Tracewaretag> TracewiretagList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Tracewaretag>();
                    var rwTable = conn.Table<Tracewaretag>().ToList();
                    var table = rwTable.Where(a => a.SiteOwnerKey == Session.ownerkey).ToList();
                    return new ObservableCollection<Tracewaretag>(table);
                }
            }
        }

        public ObservableCollection<Orientation> OrientationList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Orientation>();
                    var table = conn.Table<Orientation>().ToList();
                    return new ObservableCollection<Orientation>(table);
                }
            }
        }

        //// Cabinet Page : 
        ///  - have Manufacturer List and Model List
        ///  - didn't have Filter Type and Filter Size
        public ObservableCollection<Manufacturer> ManufacturerList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Manufacturer>();
                    var table = conn.Table<Manufacturer>().ToList();
                    foreach (var col in table)
                    {
                        col.ManufName = HttpUtility.HtmlDecode(col.ManufName); // should use for escape char "
                    }
                    return new ObservableCollection<Manufacturer>(table);
                }
            }
        }

        public ObservableCollection<DevType> ModelList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DevType>();
                    var table = conn.Table<DevType>().ToList();
                    foreach (var col in table)
                    {
                        col.DevTypeDesc = HttpUtility.HtmlDecode(col.DevTypeDesc); // should use for escape char "
                    }
                    return new ObservableCollection<DevType>(table);
                }
            }
        }

        /// <summary>
        ///  This list is for Cabinet Site Page input
        /// </summary>

        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();
                    var table = conn.Table<ModelDetail>().ToList();
                    foreach (var col in table)
                    {
                        col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                        col.ModelDescription = HttpUtility.HtmlDecode(col.ModelDescription); // should use for escape char 
                        if (col.ModelCode1 == "")
                            col.ModelCode1 = col.ModelCode2;
                        if (col.ModelCode2 == "")
                            col.ModelCode2 = col.ModelCode1;
                    }
                    Console.WriteLine();
                    return new ObservableCollection<ModelDetail>(table);
                }
            }
        }

        // "select cable_id from a_fiber_cable where OWNER_CD='$ownerCD'"
        public ObservableCollection<AFiberCable> aFiberCableList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    var table = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                    foreach (var col in table)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }

                    return new ObservableCollection<AFiberCable>(table);
                }
            }
        }

        public ObservableCollection<CableType> CableTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CableType>();
                    var table = conn.Table<CableType>().ToList();
                    return new ObservableCollection<CableType>(table);
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
                    var table = conn.Table<Sheath>().ToList();
                    return new ObservableCollection<Sheath>(table);
                }
            }
        }


        public ObservableCollection<ReelId> ReelIdList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ReelId>();
                    var rwTable = conn.Table<ReelId>().ToList();
                    var table = rwTable.Where(a => a.JobNum == Session.jobnum).ToList();
                    return new ObservableCollection<ReelId>(table);
                }
            }
        }
    }
}
