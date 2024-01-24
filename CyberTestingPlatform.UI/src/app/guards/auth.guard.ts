import { Router} from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard {
  constructor(
      private authService: AuthService,
      private router: Router
  ) { }

  canActivate(): boolean {
    if (!this.authService.isAccountConfirm()) {
      this.router.navigate(['/start']);
      return false;
    }
    return true
  }
}