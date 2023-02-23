using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

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


        void PopulateVisibleVars()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ExcludeSite>();
                var vars = conn.Table<ExcludeSite>().First();

                IsSiteNameVisible = vars.SiteName.Equals("1") ? true : false;
                Isproperty_idVisible = vars.property_id.Equals("1") ? true : false;
                IsintersectionVisible = vars.intersection.Equals("1") ? true : false;
                IsroadwayVisible = vars.roadway.Equals("1") ? true : false;
                Isdirection_of_travelVisible = vars.direction_of_travel.Equals("1") ? true : false;
                IsorientationVisible = vars.orientation.Equals("1") ? true : false;
                Issite_street_addressVisible = vars.site_street_address.Equals("1") ? true : false;
                IsdescriptionVisible = vars.description.Equals("1") ? true : false;

                IsheightVisible = vars.height.Equals("1") ? true : false;
                IsweightVisible = vars.weight.Equals("1") ? true : false;
                IswidthVisible = vars.width.Equals("1") ? true : false;
                IsdepthVisible = vars.depth.Equals("1") ? true : false;
                IsmountingVisible = vars.mounting.Equals("1") ? true : false;
                Ishas_apronVisible = vars.has_apron.Equals("1") ? true : false;
                Isapron_widthVisible = vars.apron_width.Equals("1") ? true : false;
                Isapron_heightVisible = vars.apron_height.Equals("1") ? true : false;
                Isgravel_bottomVisible = vars.gravel_bottom.Equals("1") ? true : false;
                IsHasGroundRodVisible = vars.has_ground_rod.Equals("1") ? true : false;

                IsHaveSunShield = vars.has_ground_rod.Equals("1") ? true : false;

                IsUDS_ownerVisible = vars.UDS_owner.Equals("1") ? true : false;
                IsUDS_nameVisible = vars.UDS_name.Equals("1") ? true : false;
                Isserial_numberVisible = vars.serial_number.Equals("1") ? true : false;

            }
        }
    }



}
