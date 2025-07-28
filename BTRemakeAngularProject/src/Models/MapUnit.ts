import { GPSPosition } from "./GPSPosition";
import { GUID } from "./Guid";

export interface MapUnit {
  type: 'MapUnit',
  id: GUID;
  name: string;
  position: GPSPosition;
}
