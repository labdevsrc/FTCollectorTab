using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using System.Web;
using FTCollectorApp.View.Utils;
using FTCollectorApp.Service;

namespace FTCollectorApp.View.TraceFiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DuctTracePage : ContentPage
    {
        /*public ObservableCollection<AFiberCable> aFiberCableList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    // City of Port St Lucie for Demo purpose
                    var table = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                    foreach (var col in table)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<AFiberCable>(table);
                }
            }
        }
        public ObservableCollection<DuctInstallType> DuctInstallList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctInstallType>();
                    var table = conn.Table<DuctInstallType>().ToList();
                    return new ObservableCollection<DuctInstallType>(table);
                }
            }
        }
        public ObservableCollection<UnitOfMeasure> UnitOfMeasures
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<UnitOfMeasure>();
                    var table = conn.Table<UnitOfMeasure>().ToList();
                    return new ObservableCollection<UnitOfMeasure>(table);
                }
            }
        }

        public ObservableCollection<ConduitsGroup> BeginningSiteList
        {
            get
            {
                var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                Console.WriteLine("BeginningSite");
                return new ObservableCollection<ConduitsGroup>(table);

            }
        }


        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                var table = ConduitsGroupListTable.Where(b => b.HosTagNumber == selectedTagNum).ToList();
                foreach (var col in table)
                {
                    col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                    col.WhichDucts = col.Direction + " " + col.DirCnt;
                }
                Console.WriteLine("WhichDuctLists ");
                return new ObservableCollection<ConduitsGroup>(table);
            }
        }

        public ObservableCollection<Site> Sites
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    var tableSite = conn.Table<Site>().OrderBy(g => g.TagNumber).ToList();
                    return new ObservableCollection<Site>(tableSite);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        ObservableCollection<ColorCode> ColorHextList;*/
        public DuctTracePage()
        {
            InitializeComponent();

            // the reason why put ConduitsGroupListTable instead of each subclass ex DuctConduitDatas, etc
            // to reduce code noise

            /*using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ConduitsGroup>();
                var table1 = conn.Table<ConduitsGroup>().ToList();
                ConduitsGroupListTable = new ObservableCollection<ConduitsGroup>(table1);

                conn.CreateTable<ColorCode>();
                var table2 = conn.Table<ColorCode>().ToList();
                ColorHextList = new ObservableCollection<ColorCode>(table2);
            }*/

            BindingContext = new DuctTraceViewModel();
        }

        /*protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        int selectedUOM;
        string selectedTagNum, selectedDirection, selectedDirectionCnt, selectedDuctColor, selectedHostType;
        string selectedHostTypeKey, selectedDuctInstall, selectedDuctUsage;

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }


        private void OnIndexChanged(object sender, EventArgs e)
        {

            if(pWDuct.SelectedIndex != -1)
            {
                if(pTagNumber.SelectedIndex == -1)
                {
                    DisplayAlert("Warning", "Select Beginning TagNumber First", "OK");
                    return;
                }
                var selected = pWDuct.SelectedItem as ConduitsGroup;

                selectedDuctColor = selected.DuctColor;
                selectedDirectionCnt = selected.DirCnt;
                selectedDirection = selected.Direction;
                selectedDuctUsage = selected.DuctUsage;
                txtUsage.Text = selectedDuctUsage == "1" ? "Yes" : "No";

                Console.WriteLine("");

                txtColor.BackgroundColor = Color.FromHex(ColorHextList.Where(a => a.ColorKey == selectedDuctColor).
                    Select(b => b.ColorHex).First());
                txtColor.Text = ColorHextList.Where(a => a.ColorKey == selectedDuctColor).Select(b => b.ColorName).First();
                txtColor.TextColor = Color.White;

                txtSize.Text = selected.DuctSize;
                Console.WriteLine("");
            }

            if (pCableId.SelectedIndex != -1)
            {
                var selected = pCableId.SelectedItem as AFiberCable;
                txtSMCount.Text = selected.SMCount;
                txtMMCount.Text = selected.MMCount;
            }

            if(pUOM.SelectedIndex != -1)
                selectedUOM = pUOM.SelectedIndex;

            if (pUOM.SelectedIndex != -1)
                selectedUOM = pUOM.SelectedIndex;

            if (pInstallMethod.SelectedIndex != -1)
            {
                var selected = pInstallMethod.SelectedItem as DuctInstallType;
                selectedDuctInstall = selected.DuctInstallKey;
            }
                


        }

        private void OnBeginSiteIndexChanged(object sender, EventArgs e)
        {
            if (pTagNumber.SelectedIndex != -1)
            {
                var selected = pTagNumber.SelectedItem as ConduitsGroup;
                selectedTagNum = selected.HosTagNumber;
                selectedHostType = selected.HostType;
                selectedHostTypeKey = selected.HostTypeKey;
                txtSiteType.Text = selectedHostType;

                pWDuct.ItemsSource = DuctConduitDatas; // this picker only populate after tag_number selected
                Console.WriteLine("");
            }
        }

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("host_tag_number", selectedTagNum),  // 2
                new KeyValuePair<string, string>("direction", selectedDirection),  // 3
                new KeyValuePair<string, string>("direction_count", selectedDirectionCnt),  // 4
                new KeyValuePair<string, string>("duct_size", selectedDirection),  // 5
                new KeyValuePair<string, string>("duct_color", selectedDirectionCnt),  // 6
                new KeyValuePair<string, string>("duct_type", ""),  // 6
                new KeyValuePair<string, string>("site_type_key", selectedHostTypeKey),  // 7
                new KeyValuePair<string, string>("duct_usage", selectedDuctUsage),  // 7
                new KeyValuePair<string, string>("install", selectedDuctInstall),  // 7
            };


            return keyValues;

        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostDuctTrace(KVPair);
        }*/
    }
}