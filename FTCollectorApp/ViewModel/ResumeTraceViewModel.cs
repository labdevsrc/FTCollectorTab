using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using FTCollectorApp.Service;
using FTCollectorApp.Model.AWS;
using FTCollectorApp.Model;
using System.Web;
using FTCollectorApp.View.TraceFiberPages;

namespace FTCollectorApp.ViewModel
{
    public partial class ResumeTraceViewModel: ObservableObject
    {
        [ObservableProperty]
        ConduitsGroup selectedTagNum;

        [ObservableProperty]
        bool isViewing = false;


        public ResumeTraceViewModel()
        {

            Session.current_page = "Trace";
        }

        // Auto Complete for Beginning Tag - Start
        // flag for Hide or show listview 
        [ObservableProperty]
        bool isSearching1 = false;

        [ObservableProperty]
        a_fiber_segment selectedFromSelCable;


        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(DuctConduitDatas))]
        a_fiber_segment selectedBeginSite;

        /*a_fiber_segment selectedBeginSite;
        public a_fiber_segment SelectedBeginSite
        {
            get => selectedBeginSite;

            set
            {
                Console.WriteLine();
                SetProperty(ref (selectedBeginSite), value);
                SearchFromSite = value.from_site;
                OnPropertyChanged(nameof(SearchFromSite));
            }

        }


        // search bar object
        string searchFromSite;
        public string SearchFromSite
        {
            get => searchFromSite;
            set
            {
                IsSearching1 = string.IsNullOrEmpty(value) ? false : true;
                //IsEntriesDiplayed = true;
                SetProperty(ref (searchFromSite), value);

                OnPropertyChanged(nameof(FilterCableSite));
                Console.WriteLine();
            }
        }

        // Auto Complete for Beginning Tag - End


        public ObservableCollection<a_fiber_segment> FilterCableSite
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    //var table = SQLite_a_fiber_segment.Where(a=> a.from_site == BeginningSite).To
                    conn.CreateTable<a_fiber_segment>();
                    var table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).ToList();
                    //var table4 = table3.Where(a => a.from_site == SearchBeginSite);

                    if (SearchFromSite != null)
                    {

                        table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).Where(i => i.from_site.ToLower().Contains(SearchFromSite.ToLower())).
                            GroupBy(b => b.from_site).Select(g => g.First()).ToList();
                    }
                    return new ObservableCollection<a_fiber_segment>(table);
                }
            }
        }*/

        public ObservableCollection<a_fiber_segment> DuctTagList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    //var table = SQLite_a_fiber_segment.Where(a=> a.from_site == BeginningSite).To
                    conn.CreateTable<a_fiber_segment>();
                    var table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).
                        GroupBy(b => b.from_site).Select(g => g.First()).ToList();
 
                    /*if (SearchFromSite != null)
                    {

                        table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).Where(i => i.from_site.ToLower().Contains(SearchFromSite.ToLower())).
                            GroupBy(b => b.from_site).Select(g => g.First()).ToList();
                    }*/
                    return new ObservableCollection<a_fiber_segment>(table);
                }
            }
        }


        
        ConduitsGroup selectedDuct;

        public ConduitsGroup SelectedDuct {
            get => selectedDuct;
            set
            {
                SetProperty(ref (selectedDuct), value);
                // populate FromDuct , it wiill be use in Locate point (gps_point table)and in End Trace page (a_fiber_cable)
                Console.WriteLine();
                Session.FromDuct = new ConduitsGroup
                {
                    ConduitKey = value.ConduitKey,
                    Direction = value.Direction,
                    DirCnt = value.DirCnt,
                    HosTagNumber = value.HosTagNumber,
                    HostSiteKey = value.HostSiteKey,
                    HostType = value.HostType,
                    HostTypeKey = value.HostTypeKey
                };
            }
    }

        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ConduitsGroup>();
                    
                    var table = conn.Table<ConduitsGroup>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                    if (SelectedBeginSite?.from_site != null)
                    {
                        table = conn.Table<ConduitsGroup>().Where(a => a.OwnerKey == Session.ownerkey).Where(b => b.HosTagNumber == SelectedBeginSite.from_site).ToList();
                        Console.WriteLine();
                    }
                    foreach (var col in table)
                    {
                        col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                        col.WhichDucts = col.Direction + " " + col.DirCnt;
                    }
                    Console.WriteLine("DuctConduitDatas ");
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }

        public ObservableCollection<a_fiber_segment> SelectedCableListView
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    conn.CreateTable<a_fiber_segment>();
                    var table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).ToList();

                    return new ObservableCollection<a_fiber_segment>(table);
                }
            }
        }

        [ICommand]
        async void Resume()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LocatePointPage());
            //await Application.Current.MainPage.Navigation.PushAsync(new LocatePointPage());
        }

        [ICommand]
        void ViewSuspended()
        {
            IsViewing = !IsViewing;

        }
    }
}
