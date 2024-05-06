import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AUTH_API_URL } from '../app-injection-tokens';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    @Inject(AUTH_API_URL) private authApiUrl: string,
    private http: HttpClient
  ) { }

  
  getAccounts(sampleSize: number, page: number) {
    const params = new HttpParams()
      .set('Content-Type', 'application/json')
      .set('sampleSize', sampleSize.toString())
      .set('page', page.toString());
    return this.http.get<any>(this.authApiUrl + '/Account/GetAccounts', { params });
  }
}
