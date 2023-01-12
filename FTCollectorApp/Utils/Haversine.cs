using System;

namespace FTCollectorApp.Utils
{
    /// <summary>
    /// The distance type to return the results in.
    /// </summary>
    public enum DistanceType { Miles, Kilometers };

    /// <summary>
    /// Specifies a Latitude / Longitude point.
    /// </summary>
    public struct Position
    {
        public double Latitude;
        public double Longitude;
    }

    class Haversine
    {
        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name=”pos1”></param>
        /// <param name=”pos2”></param>
        /// <param name=”type”></param>
        /// <returns></returns>
        public double Distance(Position pos1, Position pos2, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = this.toRadian(pos2.Latitude - pos1.Latitude);
            double dLon = this.toRadian(pos2.Longitude - pos1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian(pos1.Latitude)) * Math.Cos(this.toRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;



            return d;
        }

        double toRadians(double degree)
        {
            return Math.PI * degree / 180.0;
        }

        double toDegree(double rad)
        {
            return 180.0 * rad / Math.PI;
        }

        public Position NewCoordsCalc(Position pos1, double bearing, int d, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960e3 : 6371e3;
            Position newP;
            var temp = Math.Asin(Math.Sin(toRadians(pos1.Latitude)) * Math.Cos(d / R) + Math.Cos(toRadians(pos1.Latitude)) * Math.Sin(d / R) * Math.Cos(toRadians(bearing)));
            newP.Latitude = toDegree(temp);
            newP.Longitude = pos1.Longitude + toDegree(Math.Atan2(Math.Sin(toRadians(bearing)) * Math.Sin(d / R) * Math.Cos(toRadians(pos1.Latitude)),
                Math.Cos(d / R) - Math.Sin(toRadians(pos1.Latitude)) * Math.Sin(toRadians(newP.Latitude))));
            Console.WriteLine();
            return newP;
        }
        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}