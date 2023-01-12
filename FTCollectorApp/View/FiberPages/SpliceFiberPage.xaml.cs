using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.FiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpliceFiberPage : ContentPage
    {


        // $sqlsite="select * from Site where job='$job' and OWNER_CD='$ownerCD' and created_by='$uid' and record_state='L' and (type='1' or type='4') group by tag_number";
        List<string> OneTo99 = new List<string>();
        //List<string> OneToTen = new List<string>;

        public SpliceFiberPage()
        {
            InitializeComponent();
            BindingContext = new DropDownViewModel();


            for (int i = 0; i < 100; i++)
            {
                OneTo99.Add(i.ToString());
            }

            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            pFromSheathMark.ItemsSource = OneTo99;
            pFromSheathMarkEom.ItemsSource = OneTo99;
            pToSheathMark.ItemsSource = OneTo99;
            pToSheathMarkEom.ItemsSource = OneTo99;
            pToChassisMark.ItemsSource = OneTo99;
            pToChassisMarkEom.ItemsSource = OneTo99;
        }

        string selectedSiteType, selectedSiteName, fromCable, toCable;
        string fromBuffer, tobuffer, fromfiber, tofiber;
        string fromSheathMark, fromSheathMarkEom, toSheathMark, toSheathMarkEom, toChassisMark, toChassisMarkEom;
        private void OnIndexChanged(object sender, EventArgs e)
        {
            if (pSiteType.SelectedIndex != -1)
            {
                var selected = pSiteType.SelectedItem as Site;
                selectedSiteType = selected.SiteTypeKey;
            }

            if (pSiteName.SelectedIndex != -1)
            {
                var selected = pSiteName.SelectedItem as Site;
                selectedSiteName = selected.SiteName;
            }
            if (pFromCable.SelectedIndex != -1)
            {
                var selected = pFromCable.SelectedItem as AFiberCable;
                fromCable = selected.AFRKey;
            }

            if (pToCable.SelectedIndex != -1)
            {
                var selected = pToCable.SelectedItem as AFiberCable;
                toCable = selected.AFRKey;
            }
            fromSheathMark = pFromSheathMark.SelectedIndex == -1 ? "0" : pFromSheathMark.SelectedIndex.ToString();
            fromSheathMarkEom = pFromSheathMarkEom.SelectedIndex == -1 ? "0" : pFromSheathMarkEom.SelectedIndex.ToString();
            toSheathMark = pToSheathMark.SelectedIndex == -1 ? "0" : pToSheathMark.SelectedIndex.ToString();
            toSheathMarkEom = pToSheathMarkEom.SelectedIndex == -1 ? "0" : pToSheathMarkEom.SelectedIndex.ToString();

            toChassisMark = pToChassisMark.SelectedIndex == -1 ? "0" : pToChassisMark.SelectedIndex.ToString();
            toChassisMarkEom = pToChassisMarkEom.SelectedIndex == -1 ? "0" : pToChassisMarkEom.SelectedIndex.ToString();



        }






        /// 
        /// </summary>
        /// <returns></returns>

        List<KeyValuePair<string, string>> keyvaluepair()
        {


            var keyValues = new List<KeyValuePair<string, string>>{
        ///     $owner=$_POST['oid'];         //$_SESSION['oid'];
        ///     $ownerCD= $_POST['OWNER_CD']; //$_SESSION['OWNER_CD'];
        ///     $createdby= $_POST['uid'];    //$_SESSION['uid'];
        ///     $jno=$_POST['jobnum'];        //$_SESSION['jobnum'];
        ///     $stage= $_POST['stage'];      //$_SESSION['stage'];
        ///     $jobkey=$_POST['jobkey'];     //$_SESSION['jobkey'];
        ///     
                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 
                new KeyValuePair<string, string>("stage", Session.stage),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),


        /// <summary>
        /// url:"ajaxSavesplice.php",
        /// data:{"time":getCurtime(),"type":type,"fromcable":fromcable,"tocable":tocable,
        /// "frombuffer":frombuffer,"tobuffer":tobuffer,"fromfiber":fromfiber,"tofiber":tofiber,"Tray":Tray,"Slot":Slot,"splicetype":splicetype
        /// },
        ///
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 2
                new KeyValuePair<string, string>("type", selectedSiteType), //3



                new KeyValuePair<string, string>("fromcable", fromCable),  //4
                new KeyValuePair<string, string>("tocable",toCable), //8
                new KeyValuePair<string, string>("frombuffer", fromBuffer),  /// site_id
                new KeyValuePair<string, string>("tobuffer", tobuffer),  /// code_site_type.key
                new KeyValuePair<string, string>("fromfiber", fromfiber),
                new KeyValuePair<string, string>("tofiber", tofiber),

                new KeyValuePair<string, string>("Tray", ""),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("Slot", ""),
                new KeyValuePair<string, string>("splicetype", ""), /// model name, Building : x,  Cabinet/Pull Box : o

            };


            return keyValues;

        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostSaveSplice(KVPair);

        }
    }
}