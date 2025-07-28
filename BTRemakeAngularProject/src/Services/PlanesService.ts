import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { GameServerApi } from '../GameServer/GameServerApi';
import { Plane } from '../Models/Plane';
import { GUID } from '../Models/Guid';

@Injectable({
  providedIn: 'root'
})
export class PlanesService {
  private readonly endpoint = 'Planes';
  private apiService = inject(GameServerApi);

  // GET /api/Planes
  getPlanes(): Observable<Plane[]> {
    return this.apiService.getAll<Plane>(this.endpoint, "Plane");
  }

  // GET /api/Planes/{id}
  getPlaneById(id: GUID): Observable<Plane> {
    return this.apiService.getOne<Plane>(`${this.endpoint}/${id}`);
  }

  // POST /api/Planes
  createPlane(location: Partial<Plane>): Observable<Plane> {
    return this.apiService.post<Plane>(this.endpoint, location);
  }

  // PUT /api/Planes
  updatePlane(location: Partial<Plane>): Observable<Plane> {
    return this.apiService.put<Plane>(`${this.endpoint}`, location);
  }

  // DELETE /api/Planes/{id}
  deletePlane(id: GUID): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
