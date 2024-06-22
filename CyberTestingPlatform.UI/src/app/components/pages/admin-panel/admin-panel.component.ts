import { Component, ElementRef, QueryList, Renderer2, ViewChildren} from '@angular/core';
import { AccountData } from 'src/app/interfaces/accountData.model';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { AccountService } from 'src/app/services/account.service';
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

  accounts!: AccountData[];
  allCourses!: CourseData[];
  courses!: CourseData[];
  lectures!: LectureData[];
  tests!: TestData[];
  roles: string[] = [];
  pageNum: number = 1;
  pageSize: number = 24;
  searchValueAccounts: string | null = null;
  searchValueCourses: string | null = null;
  searchValueLectures: string | null = null;
  searchValueTests: string | null = null;
  searchValueResults: string | null = null;

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private accountService: AccountService,
    private renderer: Renderer2,
    private notificationService: NotificationService,
  ) {}

  async ngOnInit(): Promise<void> {
    this.checkAccountData();
    await this.getAccountsData(this.pageNum);
    await this.getCourses(this.pageNum);
    await this.getLectures(this.pageNum);
    await this.getTests(this.pageNum);
  }

  getAccountsData(pageNum: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.accounts = [];
      this.accountService.getAccounts(this.searchValueAccounts, pageNum, this.pageSize)
      .subscribe({
        next: (response: AccountData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.accounts.push(response[i]);
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

  getCourses(pageNum: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.courses = [];
      this.storageService.getCourses(this.searchValueCourses, pageNum, this.pageSize)
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
      this.storageService.getLectures(this.searchValueLectures, pageNum, this.pageSize)
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
      this.storageService.getTests(this.searchValueTests, pageNum, this.pageSize)
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

  onSearchChangedAccounts(searchValue: string) {
    this.searchValueAccounts = searchValue;
    this.getAccountsData(this.pageNum);
  }

  onSearchChangedCourses(searchValue: string) {
    this.searchValueCourses = searchValue;
    this.getCourses(this.pageNum);
  }

  onSearchChangedLectures(searchValue: string) {
    this.searchValueLectures = searchValue;
    this.getLectures(this.pageNum);
  }

  onSearchChangedTests(searchValue: string) {
    this.searchValueTests = searchValue;
    this.getTests(this.pageNum);
  }

  onSearchChangedResults(searchValue: string) {
    this.searchValueResults = searchValue;
  }

  checkAccountData() {
    var accountData = this.authService.getAccountData();
    if (accountData) {
      this.roles = accountData.role;
    } else {
      this.roles = []
    }
  }
}
