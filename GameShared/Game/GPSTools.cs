// <copyright file="GPSTools.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game
{
    internal static class GPSTools
    {
        public static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public static double ToDegree(double angle)
        {
            return (180 / Math.PI) * angle;
        }

        /// <summary>
        /// Gets the new position.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="orientationDegree">The orientation degree.</param>
        /// <param name="distanceMeters">The distance meters.</param>
        /// <returns>The new position.</returns>
        public static GPSPosition GetNewPosition(GPSPosition currentPosition, double orientationDegree, int distanceMeters)
        {
            // earth radius in meters
            const float R = 6378137;

            var orientationRad = ToRadians(orientationDegree);

            var lat_rad = ToRadians(currentPosition.Latitude);
            var lon_rad = ToRadians(currentPosition.Longitude);

            var distance_angulaire = distanceMeters / R;

            var nouvelle_lat_rad = Math.Asin(
                Math.Sin(lat_rad) * Math.Cos(distance_angulaire) +
                Math.Cos(lat_rad) * Math.Sin(distance_angulaire) * Math.Cos(orientationRad)
            );

            var nouvelle_lon_rad = lon_rad + Math.Atan2(
                Math.Sin(orientationRad) * Math.Sin(distance_angulaire) * Math.Cos(lat_rad),
                Math.Cos(distance_angulaire) - Math.Sin(lat_rad) * Math.Sin(nouvelle_lat_rad)
            );

            return new GPSPosition(ToDegree(nouvelle_lat_rad), ToDegree(nouvelle_lon_rad));
        }

        /*
# Exemple d'utilisation
lat_actuelle = 48.8566  # Paris
lon_actuelle = 2.3522
orientation = 45  # Nord-Est
distance = 1000  # 1 km

nouvelle_lat, nouvelle_lon = nouvelle_position_gps(lat_actuelle, lon_actuelle, orientation, distance)
print(f"Nouvelle position: {nouvelle_lat:.6f}, {nouvelle_lon:.6f}")
         */
    }
}
