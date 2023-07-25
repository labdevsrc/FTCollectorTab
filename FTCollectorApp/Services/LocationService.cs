/// This implementation Based on MSDN
/// 
/// https://docs.microsoft.com/en-us/xamarin/essentials/geolocation?tabs=android
/// 
////////


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FTCollectorApp.Services
{
    public static class LocationService
    {
        public static Location Coords;
        static CancellationTokenSource cts;

        public static async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: " +
                        $"{location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        public static async Task GetLocation()
        {
            try
            {
                Coords = await Geolocation.GetLastKnownLocationAsync();

                if (Coords != null)
                {
                    Console.WriteLine($"Latitude: {Coords.Latitude}, Longitude: {Coords.Longitude}, Altitude: {Coords.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine("Feature NotSupported Exception");

            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine("Feature NotEnabled Exception");

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("Permission Exception");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Unable to get location");
            }
        }
    }

  
}
