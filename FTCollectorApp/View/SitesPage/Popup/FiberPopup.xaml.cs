using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage.Popup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FiberPopup
	{
		public FiberPopup ()
		{
			InitializeComponent ();
			BindingContext = new FiberPopupVM();
		}
	}
}