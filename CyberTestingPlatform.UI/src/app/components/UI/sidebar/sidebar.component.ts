import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {

  roles : string[] = []

  constructor(
    private authService: AuthService,
  ) {}
  
  ngOnInit(): void {
    var response = this.authService.accountData();
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
