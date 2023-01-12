using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FTCollectorApp.View.BeginWork
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmentCheckIn 
    {
        public EquipmentCheckIn()
        {
            InitializeComponent();
        }

        private async void btnFinish_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

    }
}