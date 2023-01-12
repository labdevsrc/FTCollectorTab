using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View.SitesPage.Fiber
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SheathMark : ContentPage
    {
        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        ObservableCollection<ColorCode> ColorHextList;


        public SheathMark()
        {
            InitializeComponent();
            BindingContext = new SheathMarkViewModel();
        }

        private void OnIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSaveContinnue_Clicked(object sender, EventArgs e)
        {

        }

        private void btnFinishSheathMark_Clicked(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}