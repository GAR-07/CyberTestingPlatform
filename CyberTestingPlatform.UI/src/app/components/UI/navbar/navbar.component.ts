import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {

  roles : string[] = []

  constructor(
    private authService: AuthService,
  ) {}
  
  ngOnInit(): void {
    var response = this.authService.accountData();
    console.log(response);
    if (response) {
      this.roles = response.role;
    } else {
      this.roles = []
    }
  }

  logout(): void {
    this.authService.logout();
    this.ngOnInit();
  }
}
