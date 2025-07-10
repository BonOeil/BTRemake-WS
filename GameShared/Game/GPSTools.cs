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
                (Math.Sin(latRad) * Math.Cos(distanceAngulaire)) +
                (Math.Cos(latRad) * Math.Sin(distanceAngulaire) * Math.Cos(orientationRad))
            );

            var nouvelleLonRad = lonRad + Math.Atan2(
                Math.Sin(orientationRad) * Math.Sin(distanceAngulaire) * Math.Cos(latRad),
                Math.Cos(distanceAngulaire) - (Math.Sin(latRad) * Math.Sin(nouvelleLatRad))
            );

            // Normalize longitude
            nouvelleLonRad = ((nouvelleLonRad + (3 * Math.PI)) % (2 * Math.PI)) - Math.PI;

            return new GPSPosition(RadiansToDegrees(nouvelleLatRad), RadiansToDegrees(nouvelleLonRad));
        }

        /// <summary>
        /// Calcule l'angle (bearing/orientation) en degrés entre deux points GPS.
        /// L'angle est mesuré dans le sens horaire à partir du Nord (0°).
        /// </summary>
        /// <param name="position1">Starting position.</param>
        /// <param name="position2">Target position.</param>
        /// <returns>L'angle (bearing) en degrés (0° à 360°).</returns>
        public static double CalculateBearing(GPSPosition position1, GPSPosition position2)
        {
            // 1. Convertir toutes les coordonnées en radians
            double phi1 = DegreesToRadians(position1.Latitude);
            double lambda1 = DegreesToRadians(position1.Longitude);
            double phi2 = DegreesToRadians(position2.Latitude);
            double lambda2 = DegreesToRadians(position2.Longitude);

            // 2. Calculer la différence de longitude
            double deltaLambda = lambda2 - lambda1;

            // 3. Calculer les composantes X et Y pour atan2
            double X = Math.Cos(phi2) * Math.Sin(deltaLambda);
            double Y = (Math.Cos(phi1) * Math.Sin(phi2)) - (Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda));

            // 4. Calculer l'angle en radians
            double bearingRad = Math.Atan2(X, Y);

            // 5. Convertir l'angle en degrés
            double bearingDegrees = RadiansToDegrees(bearingRad);

            // 6. Normaliser l'angle pour qu'il soit entre 0 et 360 degrés
            // Math.Atan2 retourne des valeurs entre -180 et 180 degrés.
            // On ajoute 360 et on prend le modulo 360 pour s'assurer d'un résultat positif.
            bearingDegrees = (bearingDegrees + 360) % 360;

            return bearingDegrees;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }
    }
}
