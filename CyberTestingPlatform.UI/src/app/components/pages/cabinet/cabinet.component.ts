import { Component, ElementRef, QueryList, Renderer2, ViewChildren } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AccountData } from 'src/app/interfaces/accountData.model';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { TestResultData } from 'src/app/interfaces/testResultData.model';
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
  results!: TestResultData[];
  testNameOfResults!: string[];
  searchValueResults: string | null = null;
  pageNum: number = 1;
  pageSize: number = 24;

  changeProfileImgForm: FormGroup = this.formBuilder.group({
    imgPath: [null, [
      Validators.required,
      Validators.maxLength(500)
    ]],
  });
  changeLoginForm: FormGroup = this.formBuilder.group({
    userName: [null, [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(50),
      this.userNameValidator()
    ]],
  });
  changePasswordForm: FormGroup = this.formBuilder.group({
    oldPassword: [null, [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(500)
    ]],
    passwords: this.formBuilder.group({
      password: [null, [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(500)
      ]],
      confirmPassword: [null, [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(500),
      ]],
    }, { validator: this.passwordsAreEqual() })
  });

  constructor(
    private elem: ElementRef,
    private renderer: Renderer2,
    private formBuilder: FormBuilder,
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

  getAccountData(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.accountService.getAccount(this.authService.getAccountData().sub).subscribe({
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
      this.storageService.getCourses(null, this.pageSize, pageNum)
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

  getTestResultsByUser(pageNum: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.results = [];
      this.testNameOfResults = [];
      this.storageService.getTestResultsByUser(this.searchValueResults, pageNum, this.pageSize, this.account.userId)
      .subscribe({
        next: (response: TestResultData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.results.push(response[i]);
              this.getTestName(response[i].testId);
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

  onChangeProfileImgForm() {
    this.notificationService.addMessage(new NotificationMessage('Функция временно недоступна', 400));
  }

  onChangeLogin() {
    this.notificationService.addMessage(new NotificationMessage('Функция временно недоступна', 400));
  }

  onChangePasswordForm() {
    this.changePasswordForm.markAllAsTouched();
    console.log(this.changePasswordForm.valid);
    
    if (this.changePasswordForm.valid) {
      var oldPassword = this.changePasswordForm.value.oldPassword;
      var newPassword = this.changePasswordForm.value.passwords.password;
      this.authService.changePassword(oldPassword, newPassword).subscribe({
        next: () => {
          this.notificationService.addMessage(new NotificationMessage('Вы успешно сменили пароль', 200));
        },
        error: (response) => {
          console.log(response);
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
        }
      });
    }
  }
  
  removeUserCourse(id: string) {

  }

  getTestName(id: string) {
    this.storageService.getTest(id).subscribe({
      next: (response: TestData) => {
        if (response) {
          this.testNameOfResults.push(response.title);
        }
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
      }
    });
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
    localStorage.setItem('fontSize', this.currentFontSize.toString());
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

  get averageAccuracy(): number {
    if (!this.results || this.results.length === 0) {
      return 0;
    }
    const totalAccuracy = this.results.reduce((sum, result) => sum + this.calculateAccuracyPercentage(result), 0);
    return totalAccuracy / this.results.length;
  }

  calculateAccuracyPercentage(result: TestResultData): number {
    var answers = result.answers.split('\n');
    var results = result.results ? result.results.split('\n') : [];

    const correctAnswersCount = results.filter(res => res === 'Верно').length;
  
    const totalAnswersCount = answers.length;
    const accuracyPercentage = (correctAnswersCount / totalAnswersCount) * 100;
  
    return accuracyPercentage;
  }

  private userNameValidator(): ValidatorFn {
    const pattern = /[^a-zA-ZА-Яа-яЁё_]/;
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (!(control.dirty || control.touched)) {
        return null;
      }
      return pattern.test(control.value) ? { custom: 'Поле может содержать только буквы и нижнее подчёркивание' } : null;
    };
  }

  private passwordsAreEqual(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const group = control as FormGroup;
      const password = group.get('password')?.value;
      const confirmPassword = group.get('confirmPassword')?.value;

      if (!(group.dirty || group.touched) || password === confirmPassword) {
        return null;
      }
      return { custom: 'Пароли должны совпадать' };
    };
  }
}