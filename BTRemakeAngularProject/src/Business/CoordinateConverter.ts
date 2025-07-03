import { Vector3 } from "three";
import { GPSPosition } from "../Models/GPSPosition";

export class CoordinateConverter {
  static EarthRadius = 1000;//6371; // en km, à adapter selon l'échelle de votre jeu
  static Deg2Rad: number = (Math.PI / 180.0);
  static Rad2Deg: number = (180.0 / Math.PI);

  // Convertit des coordonnées GPS en position 3D
  static GpsToWorldPosition(gpsPositon: GPSPosition): Vector3 {
    // Conversion en radians
    const latRad: number = gpsPositon.latitude * this.Deg2Rad;
    const lonRad: number = gpsPositon.longitude * this.Deg2Rad;

    // Calcul de la position sur la sphère
    const x: number = (CoordinateConverter.EarthRadius + (gpsPositon.altitude ?? 0)) * Math.cos(latRad) * Math.cos(lonRad);
    const z: number = (CoordinateConverter.EarthRadius + (gpsPositon.altitude ?? 0)) * Math.cos(latRad) * Math.sin(lonRad);
    const y: number = (CoordinateConverter.EarthRadius + (gpsPositon.altitude ?? 0)) * Math.sin(latRad);

    // inverted z positionning.
    return new Vector3(x, y, -z);
  }

  static Vector3Magnitude(worldPosition: Vector3): number {
    return Math.sqrt(worldPosition.x * worldPosition.x + worldPosition.y * worldPosition.y + worldPosition.z * worldPosition.z);
  }
 
  // Convertit une position 3D en coordonnées GPS
  static WorldPositionToGps(worldPosition:Vector3): GPSPosition {
    const altitude: number = this.Vector3Magnitude(worldPosition) - CoordinateConverter.EarthRadius;

        // Calcul de la latitude et longitude
    const latitude: number = Math.asin(worldPosition.y / this.Vector3Magnitude(worldPosition)) * this.Rad2Deg;
    // inverted z positionning.
    const longitude: number = Math.atan2(-worldPosition.z, worldPosition.x) * this.Rad2Deg;

    return { latitude: latitude, longitude: longitude, altitude: altitude };
  }
}
