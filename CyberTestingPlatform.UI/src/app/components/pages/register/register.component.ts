import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { RegisterData } from 'src/app/interfaces/registerData.model';
import { AuthService } from 'src/app/services/auth.service';
import { AbstractControl, FormBuilder, FormGroup,ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { NotificationService } from 'src/app/services/notification.service';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  @ViewChild('placeholder', { static: true }) placeholder?: ElementRef;
  
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
    // role: [null, [
    //   Validators.required
    // ]],
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
    private notificationService: NotificationService,
    private renderer: Renderer2
  ) { }

  ngOnInit(): void {
    this.regForm.get('birthday')?.valueChanges.subscribe(value => {
      if (this.placeholder) {
        if (value) {
          this.renderer.setStyle(this.placeholder.nativeElement, 'display', 'none');
        } else {
          this.renderer.setStyle(this.placeholder.nativeElement, 'display', 'block');
        }
      }
    });
  }

  hidePlaceholder() {
    console.log('hidePlaceholder');

    if (this.placeholder) {
      this.renderer.setStyle(this.placeholder.nativeElement, 'display', 'none');
    }
  }

  showPlaceholder() {
    console.log('showPlaceholder');
    
    if (!this.regForm.get('birthday')?.value && this.placeholder) {
      this.renderer.setStyle(this.placeholder.nativeElement, 'display', 'block');
    }
  }

  togglePlaceholder() {
    console.log('togglePlaceholder');

    if (this.placeholder) {
      console.log(this.regForm.get('birthday'));
      if (this.regForm.get('birthday')?.value) {
        this.renderer.setStyle(this.placeholder.nativeElement, 'display', 'none');
      } else {
        this.renderer.setStyle(this.placeholder.nativeElement, 'display', 'block');
      }
    }
  }

  onSubmit() {
    this.regForm.markAllAsTouched();
    
    if (this.regForm.valid) {
      this.modelData = {
        birthday: this.regForm.value.birthday,
        userName: this.regForm.value.userName,
        email: this.regForm.value.email,
        role: 'User',
        password: this.regForm.value.passwords.password,
      };
      this.authService.register(this.modelData).subscribe({
        next: () => {
          this.notificationService.addMessage(new NotificationMessage('Вы успешно зарегистрировались', 200));
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
        }
      });
    }
  }

  private birthdayValidator(): ValidatorFn {
    const currentDate = new Date();
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (!(control.dirty || control.touched) || !control.value) {
        return null;
      }
      console.log(control.value);
      
      var inputDate = control.value.split('-');
      var birthday = new Date(inputDate[0], inputDate[1] - 1, inputDate[2]);
      
      return inputDate[0] < 1900 || birthday > currentDate ? { custom: 'Значение даты рождения не соответствует разрешённым' } : null;
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

  // formatBirthday(event: any) {
  //   var value = event.target.value.replace(/\D/g, '');
  //   var cursorPosition = event.target.selectionStart;
  
  //   if (value !== '' && !/^-$/.test(value)) {
  //     if (value.length >= 2) {
  //       value = value.substring(0, 2) + '-' + value.substring(2);
  //       if (cursorPosition === 2) {
  //         cursorPosition += 1;
  //       }
  //     }
  //     if (value.length >= 5) {
  //       value = value.substring(0, 5) + '-' + value.substring(5, 9);
  //       if (cursorPosition === 5) {
  //         cursorPosition += 1;
  //       }
  //     }

  //     if (event.inputType === 'deleteContentBackward') {
  //       if (value.length === 6) {
  //         value = value.substring(0, 5);
  //       }
  //       if (value.length === 3) {
  //         value = value.substring(0, 2);
  //       }
  //     }
  //   }

  //   this.regForm.patchValue({
  //     birthday: value,
  //   });
  //   event.target.setSelectionRange(cursorPosition, cursorPosition);
  // }
}