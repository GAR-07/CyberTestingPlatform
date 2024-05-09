import { Component, ElementRef, QueryList, Renderer2, ViewChildren } from '@angular/core';
import { AccountData } from 'src/app/interfaces/accountData.model';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { AccountService } from 'src/app/services/account.service';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-cabinet',
  templateUrl: './cabinet.component.html',
  styleUrls: ['./cabinet.component.scss']
})
export class CabinetComponent {
  @ViewChildren('tabLinks') tabLinks!: QueryList<ElementRef>;
  @ViewChildren('tabContents') tabContents!: QueryList<ElementRef>;

  currentFontSize: number = 0;
  account!: AccountData;
  courses!: CourseData[];
  pageNum: number = 1;
  pageSize: number = 24;

  constructor(
    private elem: ElementRef,
    private renderer: Renderer2,
    private authService: AuthService,
    private accountService: AccountService,
    private storageService: StorageService,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    const htmlElement = this.elem.nativeElement.ownerDocument.documentElement;
    this.currentFontSize = parseInt(window.getComputedStyle(htmlElement).fontSize, 10);
    this.getComponentData();
  }

  async getComponentData() {
    await this.getAccountData();
    console.log(this.account.userId);

    await this.getCourses(this.pageNum);
  }

  getAccountData() {
    return new Promise<void>((resolve, reject) => {
      this.accountService.getAccount(this.authService.accountData().sub).subscribe({
        next: (response: AccountData) => {
          this.account = response;    
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  getCourses(pageNum: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.courses = [];
      this.storageService.getCourses(this.pageSize, pageNum)
      .subscribe({
        next: (response: CourseData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.courses.push(response[i]);
            }
          }
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }
  
  removeUserCourse(id: string) {
    
  }

  createFilePath(serverPath: string) {
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }

  openTab(tabName: string): void {
    var open = true;
    const nestedElements = this.tabContents.first.nativeElement.querySelectorAll('.tab-content');
    nestedElements.forEach((el: HTMLElement) => {
      if (el.id === tabName) {
        if (el.style.display === 'block') {
          open = false;
          this.renderer.setStyle(el, 'display', 'none');
        } else {
          this.renderer.setStyle(el, 'display', 'block');
        }
      } else {
        this.renderer.setStyle(el, 'display', 'none');
      }
    });

    const buttons = this.tabLinks.first.nativeElement.querySelectorAll('.tab-link');
    buttons.forEach((button: HTMLElement) => {
      if (button.getAttribute('data-tab') === tabName && open) {
        this.renderer.addClass(button, 'primary');
      } else {
        this.renderer.removeClass(button, 'primary');
      }
    });
  }

  changeFontSize(delta: number): void {
    const htmlElement = this.elem.nativeElement.ownerDocument.documentElement;
    htmlElement.style.fontSize = (this.currentFontSize + delta) + 'px';
    this.currentFontSize = parseInt(window.getComputedStyle(htmlElement).fontSize, 10);
  }

  increaseFontSize(): void {
    if (this.currentFontSize < 32) {
      this.changeFontSize(2);
    }
  }

  decreaseFontSize(): void {
    if (this.currentFontSize > 8) {
      this.changeFontSize(-2);
    }
  }
}