import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { GameServerApi } from '../GameServer/GameServerApi';
import { MapUnit } from '../Models/MapUnit';

@Injectable({
  providedIn: 'root'
})
export class MapUnitsService {
  private readonly endpoint = 'MissionUnits';
  private apiService = inject(GameServerApi);

  // GET /api/MapUnits
  getUnits(): Observable<MapUnit[]> {
    return this.apiService.getAll<MapUnit>(this.endpoint, "MapUnit");
  }

  // GET /api/MapUnits/{id}
  getUnitById(id: number): Observable<MapUnit> {
    return this.apiService.getOne<MapUnit>(`${this.endpoint}/${id}`);
  }

  // POST /api/MapUnits
  createUnit(location: Partial<MapUnit>): Observable<MapUnit> {
    return this.apiService.post<MapUnit>(this.endpoint, location);
  }

  // PUT /api/MapUnits/{id}
  updateUnit(id: number, location: Partial<MapUnit>): Observable<MapUnit> {
    return this.apiService.put<MapUnit>(`${this.endpoint}/${id}`, location);
  }

  // DELETE /api/MapUnits/{id}
  deleteUnit(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
