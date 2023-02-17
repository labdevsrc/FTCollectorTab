using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.Model.Reference
{
    public class BuildingSiteDetailInfo
    {
        public string ColName { get; set; }
        public int ColType { get; set; }

        public string OwnerName { get; set; }

        public string OwnerTagNumber { get; set; }
        public BuildingType buildingType { get; set; }

        public ObservableCollection<BuildingType> ocBuildingType { get; set; }

        // add ICommand here
        // https://stackoverflow.com/questions/63124832/pass-model-to-viewmodel-on-button-click-xamarin-mvvm
        public ICommand ButtonSelectedCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var item = (e as BuildingSiteDetailInfo);
                    // logic on item
                    Console.WriteLine(item.ColName);
                });
            }
        }

    }
}
