// <copyright file="GPSTools.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game
{
    internal static class GPSTools
    {
        /// <summary>
        /// Gets the new position.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="orientationDegrees">The orientation degree.</param>
        /// <param name="distanceMeters">The distance meters.</param>
        /// <returns>The new position.</returns>
        public static GPSPosition GetNewPosition(GPSPosition currentPosition, double orientationDegrees, double distanceMeters)
        {
            // earth radius in meters
            const double R = 6378137.0;

            var orientationRad = DegreesToRadians(orientationDegrees);

            var latRad = DegreesToRadians(currentPosition.Latitude);
            var lonRad = DegreesToRadians(currentPosition.Longitude);

            var distanceAngulaire = distanceMeters / R;

            var nouvelleLatRad = Math.Asin(
                Math.Sin(latRad) * Math.Cos(distanceAngulaire) +
                Math.Cos(latRad) * Math.Sin(distanceAngulaire) * Math.Cos(orientationRad)
            );

            var nouvelleLonRad = lonRad + Math.Atan2(
                Math.Sin(orientationRad) * Math.Sin(distanceAngulaire) * Math.Cos(latRad),
                Math.Cos(distanceAngulaire) - Math.Sin(latRad) * Math.Sin(nouvelleLatRad)
            );

            return new GPSPosition(RadiansToDegrees(nouvelleLatRad), RadiansToDegrees(nouvelleLonRad));
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
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
