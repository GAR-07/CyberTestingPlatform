import { Component, ElementRef, ViewChild} from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  @ViewChild('dropdown_wrapper') dropdownWrapper!: ElementRef;
  @ViewChild('dropdown_content') dropdownContent!: ElementRef;
  
  roles : string[] = []

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.checkAccountData();
      }
    });
  }
  
  ngOnInit(): void {
    this.checkAccountData();
  }

  checkAccountData() {
    var accountData = this.authService.accountData();
    if (accountData) {
      this.roles = accountData.role;
    } else {
      this.roles = []
    }
  }

  logout(): void {
    this.authService.logout();
    this.ngOnInit();
  }

  changeNavbarType(): void {
    const wrapperElement: HTMLElement = this.dropdownWrapper.nativeElement;
    const contentElement: HTMLElement = this.dropdownContent.nativeElement;

    if (wrapperElement.classList.contains('active')) {
      wrapperElement.classList.remove('active');
      contentElement.classList.remove('active');
    } else {
      wrapperElement.classList.add('active');
      contentElement.classList.add('active');
    }
  }
}
