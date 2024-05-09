import { Component } from '@angular/core';
import { LoginData } from 'src/app/interfaces/loginData.model';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from 'src/app/services/notification.service';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  modelData = new LoginData('', '');
  loginForm: FormGroup = this.formBuilder.group({
    email: [null, [
      Validators.required,
      Validators.maxLength(1000),
      Validators.email
    ]],
    password: [null, [
      Validators.required,
      Validators.maxLength(500)
    ]],
  });

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private notificationService: NotificationService,
  ) { }

  ngOnInit(): void { }

  onSubmit() {
    this.loginForm.markAllAsTouched();

    if (this.loginForm.valid) {
      this.modelData = this.loginForm.value;
      
      this.authService.login(this.modelData).subscribe({
        next: () => {
          this.notificationService.addMessage(new NotificationMessage('Вы вошли в аккаунт', 200));
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
        }
      });
    }
  }
}