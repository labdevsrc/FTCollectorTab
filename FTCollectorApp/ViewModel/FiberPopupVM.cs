using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FTCollectorApp.ViewModel
{
    public partial class FiberPopupVM : ObservableObject
    {
        [ObservableProperty]
        string test;


        public FiberPopupVM()
        {

        }

        [ICommand]
        void Save()
        {
            Console.WriteLine();
        }

        [ICommand]
        void FiberCableBtn()
        {
        
        }

        [ICommand]
        void SheathMarkBtn()
        {

        }


        [ICommand]
        void Enclosure()
        {

        }
    }
}
