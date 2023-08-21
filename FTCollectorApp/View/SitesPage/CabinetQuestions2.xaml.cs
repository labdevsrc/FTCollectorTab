using FTCollectorApp.Model;
using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CabinetQuestions2 : ContentPage
    {
        /*public BuildingSitePageView(string minorType, string tagNumber)
        {
            InitializeComponent();
            var MajorMinorType = $"Building - {minorType}";
            BindingContext = new BuildingSitePageViewModel(MajorMinorType, tagNumber);
        }*/

        public CabinetQuestions2()
        {
            InitializeComponent();
            BindingContext = new CabinetQuestions2VM();
            Session.CabinetPage2CreateCnt = 1;
        }




    }
}