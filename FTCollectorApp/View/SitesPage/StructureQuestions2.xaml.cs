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
	public partial class StructureQuestions2 : ContentPage
	{
		public StructureQuestions2 ()
		{
			InitializeComponent ();
			BindingContext = new StructureQuestions2VM();
            Session.StructurePage2CreateCnt = 1;
        }
	}
}