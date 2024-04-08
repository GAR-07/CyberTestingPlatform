import { Component } from '@angular/core';
import { LoginData } from 'src/app/interfaces/loginData.model';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void { }

  onSubmit() {
    this.modelData = this.loginForm.value;
    this.authService.login(this.modelData).subscribe({
      next: (response: any) => {
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }
}
