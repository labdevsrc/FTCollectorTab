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
    public partial class FiberTermination : ContentPage
    {
        public FiberTermination()
        {
            InitializeComponent();
            BindingContext = new FiberTerminationViewModel();
        }
    }
}