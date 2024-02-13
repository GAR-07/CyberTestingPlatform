import { Inject, Injectable } from '@angular/core';
import { STORE_API_URL } from '../app-injection-tokens';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class StorageService {

  constructor(
    @Inject(STORE_API_URL) private authApiUrl: string,
    private http: HttpClient
    ) { }

    getCourses(sampleSize: number, page: number) {
      const params = new HttpParams()
        .set('Content-Type', 'application/json')
        .set('sampleSize', sampleSize.toString())
        .set('page', page.toString());
      return this.http.get<any>(this.authApiUrl + '/Storage/GetCourses', { params });
    }

    getLectures(sampleSize: number, page: number) {
      const params = new HttpParams()
        .set('Content-Type', 'application/json')
        .set('sampleSize', sampleSize.toString())
        .set('page', page.toString());
      return this.http.get<any>(this.authApiUrl + '/Storage/GetLectures', { params });
    }
}
