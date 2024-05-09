import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { USER_API_URL } from '../app-injection-tokens';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    @Inject(USER_API_URL) private userApiUrl: string,
    private http: HttpClient
  ) { }

  getAccount(id: string) {
    return this.http.get<any>(this.userApiUrl + '/Account/GetAccount/' + id);
  }
  
  getAccounts(searchText: string, page: number, pageSize: number) {
    const params = new HttpParams()
      .set('Content-Type', 'application/json')
      .set('searchText', searchText)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<any>(this.userApiUrl + '/Account/GetAccounts', { params });
  }
}
