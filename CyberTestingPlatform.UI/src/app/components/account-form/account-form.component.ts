import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input } from '@angular/core';
import { AccountData } from 'src/app/interfaces/accountData.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/services/notification.service';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-account-form',
  templateUrl: './account-form.component.html',
  styleUrls: ['./account-form.component.scss']
})
export class AccountFormComponent {
  @Input() mode: string = 'list';
  @Input() account!: AccountData;

  roles: string[] = [];
  imagePath: string = '';
  dragAreaClass: string = 'dragarea';
  isModalDialogVisible: boolean = false;


  accountForm: FormGroup = this.formBuilder.group({
    userName: [null, [
      Validators.required,
      Validators.maxLength(255), 
    ]],
    email: [null, [
      Validators.required,
      Validators.maxLength(255),
    ]],
    roles: [null, [
      Validators.required,
    ]],
    birthday: [null, [
      Validators.required,
    ]],
    registrationDate: [null, [
      Validators.required,
    ]]
  })

  constructor(
    private authService: AuthService,
    private accountService: AccountService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit() {
    var accountData = this.authService.getAccountData();
    this.roles = accountData ? accountData.role : [];
    this.changeMode(this.mode);
  }

  changeMode(mode: string) {
    this.mode = mode;
    this.getComponentData();
  }

  async getComponentData() {
    if(this.mode === 'edit') {
      this.accountForm.patchValue({
        userName: this.account.userName,
        email: this.account.email,
        roles: this.account.roles,
        birthday: this.account.birthday,
        registrationDate: this.account.registrationDate
      });
    }
  }

  editAccount() {
    this.notificationService.addMessage(new NotificationMessage('Функция временно недоступна', 400));
  }

  blockAccount(id: string) {
    return new Promise<void>((resolve, reject) => {
      this.accountService.BlockAccount(id)
      .subscribe({
        next: (response) => {
          this.notificationService.addMessage(new NotificationMessage('Аккаунт ' + response + ' заблокирован', 200));
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  isAccountBanned(account: { roles: string }): boolean {
    return account.roles.split(',').includes('Banned');
  }

  showModal() {
		this.isModalDialogVisible = true;
	}

  closeModal(isConfirmed: boolean) {
		this.isModalDialogVisible = false;
    if (isConfirmed === true) {
      // this.deleteLecture(this.lecture.id);
    }
	}

  createFilePath(serverPath: string) { 
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }
}
