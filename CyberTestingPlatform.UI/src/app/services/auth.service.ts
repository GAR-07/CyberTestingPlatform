import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { LoginData } from '../interfaces/loginData.model';
import { RegisterData } from '../interfaces/registerData.model';
import { AUTH_API_URL} from '../app-injection-tokens';
import { tap } from 'rxjs';

export const ACCESS_TOKEN_KEY = 'accessToken'

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(
    @Inject(AUTH_API_URL) private authApiUrl: string,
    private jwtHelper: JwtHelperService,
    private http: HttpClient,
    private router: Router
    ) { }

  login(account : LoginData) {
    return this.http.post<any>(this.authApiUrl + '/Account/Login', account).pipe(
      tap(response => {
        localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
        this.router.navigate(['']);
      })
    );
  }

  register(account : RegisterData) {
    return this.http.post<any>(this.authApiUrl + '/Account/Register', account).pipe(
      tap(response => {
        localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
        this.router.navigate(['']);
      })
    );
  }

  logout() {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    this.router.navigate(['/start']);
  }

  accountData() {
    var token = localStorage.getItem(ACCESS_TOKEN_KEY);
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return this.jwtHelper.decodeToken(token);
    }
  }

  isAccountConfirm() {
    var token = localStorage.getItem(ACCESS_TOKEN_KEY);
    return token && !this.jwtHelper.isTokenExpired(token);
  }
}
