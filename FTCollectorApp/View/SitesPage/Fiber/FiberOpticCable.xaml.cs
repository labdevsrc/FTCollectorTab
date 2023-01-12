using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.ViewModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage.Fiber
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiberOpticCable : ContentPage
    {
        List<string> TwoHundreds = new List<string>();
        string InstalledAt, Manufactured;

        ObservableCollection<CableType> cableTypeList;
        public ObservableCollection<String> cableIdList;
        public ObservableCollection<AFiberCable> FiberCableList;

        public FiberOpticCable()
        {
            InitializeComponent();
            BindingContext = new FOCViewModel();

            for (int i = 0; i < 20; i++)
            {
                TwoHundreds.Add(i.ToString());
            }

            pSingleModeCount.ItemsSource = TwoHundreds;
            pMultiModeCount.ItemsSource = TwoHundreds;
            pBufferCount.ItemsSource = TwoHundreds;
            //pMultimodeDia.ItemsSource = TwoHundreds;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Populate 

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<AFiberCable>();
                var tableAFC = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                //cableIdList = new ObservableCollection<String>();
                //cableIdList.Add("New");
                
                FiberCableList = new ObservableCollection<AFiberCable>();
                var initval = new AFiberCable { CableIdDesc = "New" };
                FiberCableList.Add(initval);

                foreach (var col in tableAFC)
                {
                    col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    //cableIdList.Add(col.CableIdDesc);
                    FiberCableList.Add(col);
                }
                

                conn.CreateTable<CableType>();
                var tableCabType = conn.Table<CableType>().ToList();
                cableTypeList =  new ObservableCollection<CableType>(tableCabType);
            }
            pCableId.ItemsSource = FiberCableList;

            pCableId.SelectedIndexChanged += OnSelectedIdxChanged;
            pCableType.SelectedIndexChanged += OnSelectedIdxChanged;
            pManufacturer.SelectedIndexChanged += OnSelectedIdxChanged;
            pModel.SelectedIndexChanged += OnSelectedIdxChanged;
            pSingleModeCount.SelectedIndexChanged += OnSelectedIdxChanged;
            pSheathTpe.SelectedIndexChanged += OnSelectedIdxChanged;

            InstalledAt = DateTime.Now.ToString("yyyy-MM-dd");
            Manufactured = DateTime.Now.ToString("yyyy-MM-dd");

        }

        string CableIdSelected, CableTypeSelected, CableTypeDescSelected, ModelSelected, SheathTypeSelected, ReelIdSelected;
        string ManufacturerSelected = null;
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostSaveFiberOpticCable(KVPair);
        }

        int SingleModeCnt =0, MultiModeCnt, BufferCount, MultiModeDiameter=0;
        private void OnSelectedIdxChanged(object sender, EventArgs e)
        {
            SingleModeCnt = pSingleModeCount.SelectedIndex == -1 ? 0 : pSingleModeCount.SelectedIndex;
            MultiModeCnt = pMultiModeCount.SelectedIndex == -1 ? 0 : pMultiModeCount.SelectedIndex;
            BufferCount = pBufferCount.SelectedIndex == -1 ? 0 : pBufferCount.SelectedIndex;
            //MultiModeDiameter = pMultimodeDia.SelectedIndex == -1 ? 0 : pMultimodeDia.SelectedIndex;
            if (pCableId.SelectedIndex != -1) {


                if (pCableId.SelectedIndex > 0){ // Other than "New"
                    var selected = pCableId.SelectedItem as AFiberCable;
                    CableIdSelected = selected.AFRKey;
                    CableTypeSelected = selected.CableType;  // HTTP POST params
                    ManufacturerSelected = selected.Manufacturer;
                    ModelSelected = selected.Manufacturer;
                    ModelSelected = selected.Model;

                    //pCableType.SelectedIndex =  int.Parse(CableTypeSelected);
                    //pCableType.IsEnabled = false;

                    // check null from cable_type
                    if (CableTypeSelected == null)
                    {
                        CableTypeSelected = "";
                        pCableType.SelectedIndex = 0;
                    }
                    else
                    {
                        pCableType.SelectedIndex = int.Parse(CableTypeSelected);
                    }



                    // check null from manufacturer_key
                    if (ManufacturerSelected == null)
                    {
                        ManufacturerSelected = "";
                        pManufacturer.SelectedIndex = 0;
                    }
                    else
                    {
                        pManufacturer.SelectedIndex = int.Parse(ManufacturerSelected);
                    }

                    // check null from model_key
                    if (ModelSelected == null)
                    {
                        ModelSelected = "";
                        pModel.SelectedIndex = 0;
                    }
                    else
                    {
                        pModel.SelectedIndex = int.Parse(ModelSelected);
                    }


                    pManufacturer.IsEnabled = false;
                    pModel.IsEnabled = false;

                }
                else
                {
                    //
                }

            }

            if (pCableType.SelectedIndex != -1)
            {
                var selected = pCableType.SelectedItem as CableType;
                CableTypeSelected = selected.CodeCableKey;
            }

            if (pManufacturer.SelectedIndex != -1)
            {
                var selected = pManufacturer.SelectedItem as Manufacturer;
                ManufacturerSelected = selected.ManufKey;
            }
            if (pModel.SelectedIndex != -1)
            {
                var selected = pModel.SelectedItem as DevType;
                ModelSelected = selected.DevTypeKey;
            }

            if (pSheathTpe.SelectedIndex != -1)
            {
                var selected = pSheathTpe.SelectedItem as Sheath;
                SheathTypeSelected = selected.SheathKey;
            }

            if (pReelId.SelectedIndex != -1)
            {
                var selected = pReelId.SelectedItem as ReelId;
                ReelIdSelected = selected.ReelKey;
            }
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            InstalledAt = pInstalledAt.Date.ToString("yyyy-MM-dd");
            Manufactured = pManufacturedAt.Date.ToString("yyyy-MM-dd");
        }


        List<KeyValuePair<string, string>> keyvaluepair()
        {

            var keyValues = new List<KeyValuePair<string, string>>{
  
                new KeyValuePair<string, string>("cable_id", CableIdSelected), //#2
                new KeyValuePair<string, string>("manufacturer", ManufacturerSelected), 
                new KeyValuePair<string, string>("model", ModelSelected), 
                new KeyValuePair<string, string>("manufactured_date", ""), 
                new KeyValuePair<string, string>("label", ""), 
                new KeyValuePair<string, string>("cablelen", ""), 



                new KeyValuePair<string, string>("singlemode_count", SingleModeCnt.ToString()), //3
                new KeyValuePair<string, string>("multimode_count", MultiModeCnt.ToString()),  //4
                new KeyValuePair<string, string>("buffer_count", BufferCount.ToString()), //1
                //new KeyValuePair<string, string>("owner", Session.ownerkey), //5
                new KeyValuePair<string, string>("reel", ReelIdSelected), // 6
                new KeyValuePair<string, string>("installed_date",InstalledAt), //  7 

                new KeyValuePair<string, string>("cabtype",CableTypeSelected), //8
                new KeyValuePair<string, string>("installtyp", ""),  /// site_id
                new KeyValuePair<string, string>("sheath", SheathTypeSelected),  /// code_site_type.key
                new KeyValuePair<string, string>("multimode_diameter", ""),

                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // #1
                //new KeyValuePair<string, string>("accuracy", Session.accuracy), //3
                //new KeyValuePair<string, string>("altitude", Session.altitude),  //4
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 
                new KeyValuePair<string, string>("stage", Session.stage),
                new KeyValuePair<string, string>("country", ""),

            };


            return keyValues;

        }

    }
}