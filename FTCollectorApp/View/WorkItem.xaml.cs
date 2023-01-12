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
    public partial class WorkItem : MasterDetailPage
    {
        List<WorkStep> workStep;
        public WorkItem()
        {
            InitializeComponent();
            workStep = new List<WorkStep>();

        }
    }

    public class WorkStep {
        public string WorkStepItem { get; set; }
    }
}