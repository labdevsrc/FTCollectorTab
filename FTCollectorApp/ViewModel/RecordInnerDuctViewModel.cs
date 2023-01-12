using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using FTCollectorApp.Model.Reference;
using System.Collections.ObjectModel;
using SQLite;
using FTCollectorApp.Model;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class RecordInnerDuctViewModel : ObservableObject
    {
        public RecordInnerDuctViewModel()
        {
            Session.current_page = "DuctInner";
        }


        [ObservableProperty]
        ConduitsGroup hostTag;

        [ObservableProperty]
        CompassDirection selectedDirection;

        [ObservableProperty]
        string ductDirNum;

        [ObservableProperty]
        string selectedDuctSize;

        [ObservableProperty]
        string selectedDuctColor;

        [ObservableProperty]
        string selectedDuctType;

        [ObservableProperty]
        string selectedDuctUsage;
        
        [ObservableProperty]
        bool inUseIsChecked;

        [ICommand]
        async void Submit()
        {
            await Application.Current.MainPage.DisplayAlert("Page Under Construction", "This button have no activity", "OK");
        }

        [ICommand]
        async void Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }


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

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("host_tag_number", Session.tag_number),  // 2
                new KeyValuePair<string, string>("direction", SelectedDirection?.CompasKey  == null ? "0": SelectedDirection.CompasKey),  // 3
                //new KeyValuePair<string, string>("direction_count", SelectedDirectionCnt ??= "0"),  // 4
                new KeyValuePair<string, string>("duct_size",  SelectedDuctSize), // SelectedDuctSize?.DuctKey == null ? "0": SelectedDuctSize.DuctKey),  // 5
                new KeyValuePair<string, string>("duct_color", SelectedDuctColor), //SelectedColor?.ColorKey == null ? "0": SelectedColor.ColorKey),  // 6
                new KeyValuePair<string, string>("duct_type",  SelectedDuctType), //selectedDuctType?.DucTypeKey == null ?"0" : SelectedDuctType.DucTypeKey),  // 7
                //new KeyValuePair<string, string>("site_type_key", Session.site_type_key),  // 8
                new KeyValuePair<string, string>("in_use", InUseIsChecked ? "0" : "1"),  // 9
                new KeyValuePair<string, string>("duct_usage", InUseIsChecked ? "0" : "1"),  // 9


            };


            return keyValues;

        }
    }
}
