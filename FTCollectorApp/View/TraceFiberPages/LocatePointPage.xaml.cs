using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.TraceFiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocatePointPage : ContentPage
    {
        public LocatePointPage()
        {
            InitializeComponent();
            try
            {
                BindingContext = new LocatePointViewModel();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}