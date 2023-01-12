using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.View.TraceFiberPages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class TracingMenuViewModel : ObservableObject
    {
        [ICommand]
        async void StartTracer()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new DuctTracePage());
        }


        [ICommand]
        async void ResumeTracer()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ResumeTracer());
        }
    }
}
