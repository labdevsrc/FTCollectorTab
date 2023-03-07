using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiberInstallTabPage : TabbedPage
    {
        public FiberInstallTabPage()
        {
            InitializeComponent();
        }

        private void NewFiberClicked(object sender, EventArgs e)
        {
            var tabbedpage = this.Parent as TabbedPage;
            tabbedpage.CurrentPage = Children[0];
        }
    }
}