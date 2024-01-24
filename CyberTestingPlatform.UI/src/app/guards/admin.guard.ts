import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})

export class AdminGuard {
  constructor(
      private authService: AuthService,
      private router: Router
  ) { }

  canActivate(): boolean {
    if (!this.authService.isAccountConfirm()) {
      this.router.navigate(['/start']); 
      return false
    }
    if (this.authService.accountData().role) {
      alert('Недостаточный уровень доступа аккаунта');
      this.router.navigate(['']);
      return false
    }
    return true
  }
}