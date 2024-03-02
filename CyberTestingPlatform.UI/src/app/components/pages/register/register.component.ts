import { Component } from '@angular/core';
import { RegisterData } from 'src/app/interfaces/registerData.model';
import { AuthService } from 'src/app/services/auth.service';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {

  modelData = new RegisterData('', '', '', '', '');
  regForm: FormGroup = this.formBuilder.group({
    birthday: [null, [
      Validators.required, 
      this.birthdayValidator()
    ]],
    email: [null, [
      Validators.required,
      Validators.maxLength(1000),
      Validators.email
    ]],
    userName: [null, [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(50),
      this.userNameValidator()
    ]],
    role: [null, [
      Validators.required
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
    private authService: AuthService,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit(): void { }

  onSubmit() {
    this.modelData = {
      birthday: this.regForm.value.birthday,
      userName: this.regForm.value.userName,
      email: this.regForm.value.email,
      role: this.regForm.value.role,
      password: this.regForm.value.passwords.password,
    };
    this.authService.register(this.modelData).subscribe({
      next: (response: any) => {
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
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