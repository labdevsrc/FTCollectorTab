using System.Web;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using SQLite;
using Xamarin.Forms;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Model;
using System.Collections.ObjectModel;
using FTCollectorApp.View.Utils;
using FTCollectorApp.View.SitesPage.Popup;

namespace FTCollectorApp.ViewModel
{
    public partial class PullBoxQuestions2VM : ObservableObject
    {
        [ObservableProperty] string diameter;
        [ObservableProperty] bool isHasKey = false;
        [ObservableProperty] bool isSpliceVault = false;
        [ObservableProperty] bool isHasGround = false;

        bool isKeyTypeDisplay = false;
        public bool IsKeyTypeDisplay
        {
            get => isKeyTypeDisplay;
            set
            {
                if (IsHasKey)
                    SetProperty(ref isKeyTypeDisplay, true);
                else
                    SetProperty(ref isKeyTypeDisplay, false);
            }
        }
        public ICommand CaptureCommand { get; set; }
        public ICommand CompleteSiteCommand { get; set; }

        public PullBoxQuestions2VM()
        {
            CaptureCommand = new Command(
                execute: async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
                }
            );
            CompleteSiteCommand = new Command(
                execute: async () =>
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new CompleteSitePopUp());
                },
                canExecute: () =>
                {
                    return Session.SiteCreateCnt > 0;
                }
            );
        }


        [ObservableProperty]
        Mounting selectedMounting;

        public ObservableCollection<MaterialCode> MaterialCodeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<MaterialCode>();
                    var table = conn.Table<MaterialCode>().ToList();
                    foreach (var col in table)
                    {
                        col.CodeDescription = HttpUtility.HtmlDecode(col.CodeDescription); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<MaterialCode>(table);
                }
            }
        }
        public ObservableCollection<Mounting> MountingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Mounting>();
                    var mountingTable = conn.Table<Mounting>().OrderBy(a => a.MountingType).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<Mounting>(mountingTable);

                }
            }
        }
    }
}