using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using FTCollectorApp.Model.Reference;
using System.Collections.ObjectModel;
using System.Linq;
using FTCollectorApp.Model;

namespace FTCollectorApp.ViewModel
{
    public partial class BuildingSiteItemsVM : ObservableObject
    {
        /// Roadway - intersection Properties - start

        [ObservableProperty]
        bool isSearchingRoadway = false;

        //[ObservableProperty]
        InterSectionRoad selectedIntersection;
        public InterSectionRoad SelectedIntersection
        {
            get => selectedIntersection;
            set
            {
                SetProperty(ref selectedIntersection, value);
                IsSearchingRoadway = false;
            }
        }


        Roadway selectedRoadway;
        public Roadway SelectedRoadway
        {
            get => selectedRoadway;
            set
            {
                SetProperty(ref (selectedRoadway), value);
                SearchRoadway = value.RoadwayName;
                OnPropertyChanged(nameof(IntersectionList));
                OnPropertyChanged(nameof(SearchRoadway));
            }
        }

        string searchRoadway;
        public string SearchRoadway
        {
            get => searchRoadway;
            set
            {
                IsSearchingRoadway = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchRoadway), value);
                OnPropertyChanged(nameof(IntersectionList));
                Console.WriteLine();
            }
        }

        public ObservableCollection<Roadway> RoadwayList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<Roadway>();
                    var rwTable = conn.Table<Roadway>().ToList();
                    var table = rwTable.Where(a => a.RoadOwnerKey == Session.ownerkey).ToList();
                    if (SearchRoadway != null)
                    {
                        Console.WriteLine();
                        table = conn.Table<Roadway>().Where(i => i.RoadwayName.ToLower().Contains(SearchRoadway.ToLower())).
                            GroupBy(b => b.RoadwayName).Select(g => g.First()).ToList();
                    }
                    Console.WriteLine();
                    return new ObservableCollection<Roadway>(table);
                }
            }
        }

        public ObservableCollection<InterSectionRoad> IntersectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<InterSectionRoad>();
                    var data = conn.Table<InterSectionRoad>().ToList();
                    if (SelectedRoadway != null)
                    {
                        data = conn.Table<InterSectionRoad>().Where(b => b.major_roadway == SelectedRoadway.RoadwayKey || b.minor_roadway == SelectedRoadway.RoadwayKey).ToList();
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    //var data = conn.Table<InterSectionRoad>().Where(a => a.OWNER_CD == Session.ownerCD).GroupBy(b => b.IntersectionName).Select(g => g.First()).ToList();
                    return new ObservableCollection<InterSectionRoad>(data);
                }
            }
        }
        /// Roadway - intersection - end
        /// 

        public ObservableCollection<CompassDirection> TravelDirectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<CompassDirection>();
                    var data = conn.Table<CompassDirection>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<CompassDirection>(data);
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


        /// new feature 2102
        /// add exclude cols

        /// start
        /// 
        [ObservableProperty]
        bool isSiteNameVisible = false;

        [ObservableProperty]
        bool isproperty_idVisible = false;

        [ObservableProperty]
        bool isintersectionVisible = false;

        [ObservableProperty]
        bool isroadwayVisible = false;

        [ObservableProperty]
        bool isdirection_of_travelVisible = false;

        [ObservableProperty]
        bool isorientationVisible = false;

        [ObservableProperty]
        bool issite_street_addressVisible = false;

        [ObservableProperty]
        bool isdescriptionVisible = false;

        [ObservableProperty]
        bool isdepthVisible = false;

        [ObservableProperty]
        bool isheightVisible = false;

        [ObservableProperty]
        bool isweightVisible = false;

        [ObservableProperty]
        bool iswidthVisible = false;

        [ObservableProperty]
        bool isdiameterVisible = false;

        [ObservableProperty]
        bool ismountingVisible = false;


        [ObservableProperty]
        bool islane_closure_requiredVisible = false;


        [ObservableProperty]
        bool isHasPowerDisConnectedVisible = false;


        [ObservableProperty]
        bool ishas_commsVisible = false;

        [ObservableProperty]
        bool iscomms_providerVisible = false;

        [ObservableProperty]
        bool isUDS_ownerVisible = false;

        [ObservableProperty]
        bool isUDS_nameVisible = false;

        [ObservableProperty]
        bool isFilterSizeVisible = false;

        [ObservableProperty]
        bool isFilterTypeVisible = false;



        [ObservableProperty]
        bool isapron_widthVisible = false;

        [ObservableProperty]
        bool ishas_apronVisible = false;

        [ObservableProperty]
        bool isapron_heightVisible = false;

        [ObservableProperty]
        bool isgravel_bottomVisible = false;

        [ObservableProperty]
        bool isHasGroundRodVisible = false;

        [ObservableProperty]
        bool isHaveSunShield = false;

        [ObservableProperty]
        bool isGroundResistanceVisible = false;


        [ObservableProperty]
        bool isSiteStreetAddrVisible = false;

        [ObservableProperty]
        bool isserial_numberVisible = false;

        public BuildingSiteItemsVM()
        {
            PopulateVisibleVars();
        }

        void PopulateVisibleVars()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ExcludeSite>();
                var vars = conn.Table<ExcludeSite>().First();

                IsSiteNameVisible = vars.SiteName.Equals("0");
                Isproperty_idVisible = vars.property_id.Equals("0");
                IsintersectionVisible = vars.intersection.Equals("0");
                IsroadwayVisible = vars.roadway.Equals("0");
                Isdirection_of_travelVisible = vars.direction_of_travel.Equals("0");
                IsorientationVisible = vars.orientation.Equals("0");
                Issite_street_addressVisible = vars.site_street_address.Equals("0");
                IsdescriptionVisible = vars.description.Equals("0");

                IsheightVisible = vars.height.Equals("0");
                IsweightVisible = vars.weight.Equals("0");
                IswidthVisible = vars.width.Equals("0");
                IsdepthVisible = vars.depth.Equals("0");
                IsmountingVisible = vars.mounting.Equals("0");
                Ishas_apronVisible = vars.has_apron.Equals("0");
                Isapron_widthVisible = vars.apron_width.Equals("0");
                Isapron_heightVisible = vars.apron_height.Equals("0");
                Isgravel_bottomVisible = vars.gravel_bottom.Equals("0");
                IsHasGroundRodVisible = vars.has_ground_rod.Equals("0");

                IsHaveSunShield = vars.has_ground_rod.Equals("0");

                IsUDS_ownerVisible = vars.UDS_owner.Equals("0");
                IsUDS_nameVisible = vars.UDS_name.Equals("0");
                Isserial_numberVisible = vars.serial_number.Equals("0");

            }
        }
    }



}
