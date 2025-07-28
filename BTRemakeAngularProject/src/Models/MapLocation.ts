import { GPSPosition } from "./GPSPosition";
import { GUID } from "./Guid";

export interface MapLocation {
  type: 'MapLocation';
  id: GUID;
  name: string;
  position: GPSPosition;
}
