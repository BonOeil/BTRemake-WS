import { GUID } from "./Guid";

export interface Plane {
  type: 'Plane',
  id?: GUID;
  name: string;
}
