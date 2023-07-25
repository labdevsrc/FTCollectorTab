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
	public partial class CreateSitewQuestion1 : ContentPage
	{
		public CreateSitewQuestion1 ()
		{
			InitializeComponent ();
			BindingContext = new CreateSitewQuestion1VM();

        }
	}
}