import { Vector3 } from "three";

export type GpsPosition = {
  latitude: number, longitude: number, altitude: number
};

export class CoordinateConverter {
  static EarthRadius: number = 1000;//6371; // en km, à adapter selon l'échelle de votre jeu
  static Deg2Rad: number = (Math.PI / 180.0);
  static Rad2Deg: number = (180.0 / Math.PI);

  // Convertit des coordonnées GPS en position 3D
  static GpsToWorldPosition(gpsPositon: GpsPosition): Vector3 {
    // Conversion en radians

    var latRad: number = gpsPositon.latitude * this.Deg2Rad;
    var lonRad: number = gpsPositon.longitude * this.Deg2Rad;

        // Calcul de la position sur la sphère
    var x: number = (CoordinateConverter.EarthRadius + gpsPositon.altitude) * Math.cos(latRad) * Math.cos(lonRad);
    var z: number = (CoordinateConverter.EarthRadius + gpsPositon.altitude) * Math.cos(latRad) * Math.sin(lonRad);
    var y: number = (CoordinateConverter.EarthRadius + gpsPositon.altitude) * Math.sin(latRad);

    return new Vector3(x, y, z);
  }

  static Vector3Magnitude(worldPosition: Vector3): number {
    return (worldPosition.x * worldPosition.x + worldPosition.y * worldPosition.y + worldPosition.z * worldPosition.z);
  }
 
  // Convertit une position 3D en coordonnées GPS
  static WorldPositionToGps(worldPosition:Vector3): GpsPosition {
    var altitude: number = this.Vector3Magnitude(worldPosition) - CoordinateConverter.EarthRadius;

        // Calcul de la latitude et longitude
    var latitude: number = Math.asin(worldPosition.y / this.Vector3Magnitude(worldPosition)) * this.Rad2Deg;
    var longitude: number = Math.atan2(worldPosition.z, worldPosition.x) * this.Rad2Deg;

    return { latitude: latitude, longitude: longitude, altitude: altitude };
  }
}
