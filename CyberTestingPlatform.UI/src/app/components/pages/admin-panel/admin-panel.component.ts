import { Component, ElementRef, QueryList, Renderer2, ViewChildren} from '@angular/core';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent {
  @ViewChildren('tabLinks') tabLinks!: QueryList<ElementRef>;
  @ViewChildren('tabContents') tabContents!: QueryList<ElementRef>;

  allCourses!: CourseData[];
  courses!: CourseData[];
  lectures!: LectureData[];
  tests!: TestData[];
  pageNum: number = 1;
  pageSize: number = 24;
  roles : string[] = []

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private renderer: Renderer2,
    private notificationService: NotificationService,
  ) {}

  async ngOnInit(): Promise<void> {
    this.checkAccountData();
    await this.getCourses(this.pageNum);
    await this.getLectures(this.pageNum);
    await this.getTests(this.pageNum);
  }

  // getAccountsData() {
  //   return new Promise<void>((resolve, reject) => {
  //     this.accountService.getAccounts('m', 1, 10).subscribe({
  //       next: (response: AccountData[]) => {
  //         console.log(response);
  //         resolve();
  //       },
  //       error: (response) => {
  //         this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
  //         reject();
  //       }
  //     });
  //   });
  // }

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

  getLectures(pageNum: number) {
    return new Promise<void>((resolve, reject) => {
      this.lectures = [];
      this.storageService.getLectures(this.pageSize, pageNum)
      .subscribe({
        next: (response: LectureData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.lectures.push(response[i]);
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

  getTests(pageNum: number) {
    return new Promise<void>((resolve, reject) => {
      this.tests = [];
      this.storageService.getTests(this.pageSize, pageNum)
      .subscribe({
        next: (response: TestData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.tests.push(response[i]);
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

  checkAccountData() {
    var accountData = this.authService.accountData();
    if (accountData) {
      this.roles = accountData.role;
    } else {
      this.roles = []
    }
  }
}
