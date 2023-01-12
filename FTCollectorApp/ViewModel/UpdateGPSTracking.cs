using FTCollectorApp.Model;
using FTCollectorApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public class UpdateGPSTracking
    {
        public async Task RunUpdateGPSTracking(CancellationToken token)
        {
            await Task.Run(async () => {


                token.ThrowIfCancellationRequested();

                await Task.Delay(60000);

                
                Position coords = new Position();
                coords.Latitude = double.Parse(Session.lattitude_offset);
                coords.Longitude = double.Parse(Session.longitude_offset);

                Device.BeginInvokeOnMainThread(() =>
                {
                    //MessagingCenter.Send<Position>(coords, "TickedCoords");
                    // TODO Update gps_tracking 
                });

            }, token);
        }
    }
}
