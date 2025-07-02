import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { GameServerApi } from '../GameServer/GameServerApi';
import { MapLocation } from '../Models/MapLocation';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private readonly endpoint = 'Location';
  private apiService = inject(GameServerApi);

  // GET /api/locations
  getLocations(): Observable<MapLocation[]> {
    return this.apiService.get<MapLocation[]>(this.endpoint);
  }

  // GET /api/locations/{id}
  getLocationById(id: number): Observable<MapLocation> {
    return this.apiService.get<MapLocation>(`${this.endpoint}/${id}`);
  }

  // POST /api/locations
  createLocation(location: Partial<MapLocation>): Observable<MapLocation> {
    return this.apiService.post<MapLocation>(this.endpoint, location);
  }

  // PUT /api/locations/{id}
  updateLocation(id: number, location: Partial<MapLocation>): Observable<MapLocation> {
    return this.apiService.put<MapLocation>(`${this.endpoint}/${id}`, location);
  }

  // DELETE /api/locations/{id}
  deleteLocation(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
