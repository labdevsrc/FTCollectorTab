using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrewCRUDPopUp 
	{
		public CrewCRUDPopUp()
		{
			InitializeComponent();
			BindingContext = new CrewCRUDVM();
		}
	}
}