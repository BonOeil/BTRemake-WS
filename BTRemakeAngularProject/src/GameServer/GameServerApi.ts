import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameServerApi {
  //private apiUrl = "https://btremake-srvapp--0000002.redstone-6d485465.francecentral.azurecontainerapps.io";
  private apiUrl = "http://localhost:8080";

  private http = inject(HttpClient);

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
  }

  // GET request
  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/api/${endpoint}`, {
      headers: this.getHeaders()
    });
  }

  // POST request
  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/api/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }

  // PUT request
  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/api/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }

  // DELETE request
  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/api/${endpoint}`, {
      headers: this.getHeaders()
    });
  }
}
