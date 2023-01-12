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
using FTCollectorApp.Model.Reference;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EqCheckOutPage : ContentPage
    {
        int selectedTypeIdx = 0;

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
                    var selected = selectedTypeIdx.ToString();
                    var table = conn.Table<EquipmentDetailType>().Where(g => g.EquipmentType == selected).ToList();
                    return new ObservableCollection<EquipmentDetailType>(table);
                }
            }
        }

        public ObservableCollection<EquipmentDetailType> EquipmentDetailAssets
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<EquipmentDetailType>();
                    var table = conn.Table<EquipmentDetailType>().OrderBy(a => a.EquipmentNumber).ToList();
                    return new ObservableCollection<EquipmentDetailType>(table);
                }
            }
        }

        public EqCheckOutPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        private void btnFinish_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void btnLogOut_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }


        private void OnIndexChanged(object sender, EventArgs e)
        {
            if (pType1.SelectedIndex != -1)
            {
                selectedTypeIdx = pType1.SelectedIndex + 1;
                pDesc1.ItemsSource = EquipmentDetailTypes;
                Console.WriteLine("OnIndexChanged pType1");
            }

            if (pType2.SelectedIndex != -1)
            {
                selectedTypeIdx = pType2.SelectedIndex + 1;
                pDesc2.ItemsSource = EquipmentDetailTypes;
                Console.WriteLine("OnIndexChanged pType2");
            }

            if (pType3.SelectedIndex != -1)
            {
                selectedTypeIdx = pType3.SelectedIndex + 1;
                pDesc3.ItemsSource = EquipmentDetailTypes;
                Console.WriteLine("OnIndexChanged pType3");
            }

            if (pType4.SelectedIndex != -1)
            {
                selectedTypeIdx = pType4.SelectedIndex + 1;
                pDesc4.ItemsSource = EquipmentDetailTypes;
                Console.WriteLine("OnIndexChanged pType4");
            }

            if (pType5.SelectedIndex != -1)
            {
                selectedTypeIdx = pType5.SelectedIndex + 1;
                pDesc5.ItemsSource = EquipmentDetailTypes;
                Console.WriteLine("OnIndexChanged pType5");
            }

            if (pType6.SelectedIndex != -1)
            {
                selectedTypeIdx = pType6.SelectedIndex + 1;
                pDesc6.ItemsSource = EquipmentDetailTypes;
                Console.WriteLine("OnIndexChanged pType6");
            }
        }
    }
}