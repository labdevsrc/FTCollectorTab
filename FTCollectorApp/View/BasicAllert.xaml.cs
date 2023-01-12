using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;
using FTCollectorApp.Model.Reference;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasicAllert 
    {
        public ICommand GetDialogResultCommand { get; set; }

        BasicAllertResult Result = new BasicAllertResult() ;
        public BasicAllert(string strMessage, string strTitle)
        {
            messageTxt = strMessage;
            titlePopUp = strTitle;
            InitializeComponent();
            BindingContext = this;

            Result.OK = false;
        }

        public BasicAllert(string strMessage, string strTitle, bool OK )
        {
            messageTxt = strMessage;
            titlePopUp = strTitle;
            InitializeComponent();
            BindingContext = this;
            Result.OK = OK;

        }

        /*bool selectedOK = false;
        public bool SelectedOK
        {
            get => selectedOK;
            set
            {
                if (selectedOK == value) return;
                selectedOK = value;
                OnPropertyChanged(nameof(SelectedOK));
                Console.WriteLine();
            }
        }*/

        /*public BasicAllertResult AllertResult
        {
            get => allertResult;
            set
            {
                if (allertResult == value)
                    return;
                allertResult = value;
                OnPropertyChanged(nameof(BasicAllertResult));
            }
        }*/


        string messageTxt;
        public string MessageTxt
        {
            get => messageTxt;
            set
            {
                if (messageTxt == value)
                    return;
                messageTxt = value;
                OnPropertyChanged(nameof(MessageTxt));
            }
        }


        string titlePopUp;
        public string TitlePopUp
        {
            get => titlePopUp;
            set
            {
                if (titlePopUp == value)
                    return;
                titlePopUp = value;
                OnPropertyChanged(nameof(TitlePopUp));
            }
        }

        private async void OnOK(object sender, EventArgs e)
        {
            Console.WriteLine();
            Result.OK = true;
            Console.WriteLine();
            GetDialogResultCommand?.Execute(Result);
            
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void OnCancel(object sender, EventArgs e)
        {
            Result.OK = false;
            Console.WriteLine();
            GetDialogResultCommand?.Execute(Result);

            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}