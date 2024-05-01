import { Component } from '@angular/core';
import { RegisterData } from 'src/app/interfaces/registerData.model';
import { AuthService } from 'src/app/services/auth.service';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { NotificationService } from 'src/app/services/notification.service';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  modelData = new RegisterData('', '', '', '', '');
  regForm: FormGroup = this.formBuilder.group({
    birthday: ['2023-10-12', [
      Validators.required, 
      this.birthdayValidator()
    ]],
    email: ['ya@ya.ru', [
      Validators.required,
      Validators.maxLength(1000),
      Validators.email
    ]],
    userName: ['newUser', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(50),
      this.userNameValidator()
    ]],
    role: ['User', [
      Validators.required
    ]],
    passwords: this.formBuilder.group({
      password: ['123123123', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(500)
      ]],
      confirmPassword: ['123123123', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(500),
      ]],
    }, { validator: this.passwordsAreEqual() })
  });

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private notificationService: NotificationService,
  ) { }

  ngOnInit(): void { }

  onSubmit() {
    if (this.regForm.valid) {
      this.modelData = {
        birthday: this.regForm.value.birthday,
        userName: this.regForm.value.userName,
        email: this.regForm.value.email,
        role: this.regForm.value.role,
        password: this.regForm.value.passwords.password,
      };
      this.authService.register(this.modelData).subscribe({
        next: () => {
          this.notificationService.addMessage(new NotificationMessage('success', 'Вы успешно зарегистрировались'));
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
        }
      });
    }
  }

  private birthdayValidator(): ValidatorFn {
    const currentDate = new Date();
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (!(control.dirty || control.touched)) {
        return null;
      }
      var inputDate = control.value.split('-');
      var birthday = new Date(inputDate[0], inputDate[1], inputDate[2]);
      
      return inputDate[0] < 1900 || birthday > currentDate ? { custom: 'Значение даты не соответствует разрешённым' } : null;
    };
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