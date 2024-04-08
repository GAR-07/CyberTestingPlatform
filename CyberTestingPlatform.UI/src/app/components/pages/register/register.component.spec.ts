import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterComponent } from './register.component';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AUTH_API_URL } from 'src/app/app-injection-tokens';
import { of } from 'rxjs';
import { ValidatorMessageComponent } from '../../UI/validator-message/validator-message.component';
import { RegisterData } from 'src/app/interfaces/registerData.model';
import { Inject } from '@angular/core';


describe('RegisterComponent', () => {
  let authService: AuthService;
  let formBuilder: FormBuilder;
  let authApiUrl: string = Inject(AUTH_API_URL);
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;

  beforeEach(async () => {
    authService = jasmine.createSpyObj('AuthService', ['register']);
    TestBed.configureTestingModule({
      declarations: [RegisterComponent, ValidatorMessageComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authService },
        { provide: AUTH_API_URL, useValue:  authApiUrl}
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    formBuilder = TestBed.inject(FormBuilder);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should register', () => {
    const formData = {
      birthday: '01/01/2000',
      userName: 'testUser',
      email: 'test@example.com',
      role: 'user',
      passwords: {
        password: 'password',
        confirmPassword: 'password'
      }
    };
    component.regForm.setValue(formData);
    component.regForm.updateValueAndValidity();
    
    const registerData: RegisterData = {
      birthday: formData.birthday,
      userName: formData.userName,
      email: formData.email,
      role: formData.role,
      password: formData.passwords.password
    };
  
    const response = { success: true };
    (authService.register as jasmine.Spy).and.returnValue(of(response));
    component.onSubmit();
  
    expect(authService.register).toHaveBeenCalledWith(registerData);
  });

  it('not should register with bad email', () => {
    const formData = {
      birthday: '01/01/2000',
      userName: 'testUser',
      email: 'test!example.com',
      role: 'user',
      passwords: {
        password: 'password',
        confirmPassword: 'password'
      }
    };
    component.regForm.setValue(formData);
    component.regForm.updateValueAndValidity();
    
    const registerData: RegisterData = {
      birthday: formData.birthday,
      userName: formData.userName,
      email: formData.email,
      role: formData.role,
      password: formData.passwords.password
    };
  
    const response = { success: true };
    (authService.register as jasmine.Spy).and.returnValue(of(response));
    component.onSubmit();
  
    expect(authService.register).not.toHaveBeenCalledWith(registerData);
  });

  it('not should register with bad password', () => {
    const formData = {
      birthday: '01/01/2000',
      userName: 'testUser',
      email: 'test@example.com',
      role: 'user',
      passwords: {
        password: 'pass',
        confirmPassword: 'pass'
      }
    };
    component.regForm.setValue(formData);
    component.regForm.updateValueAndValidity();
    
    const registerData: RegisterData = {
      birthday: formData.birthday,
      userName: formData.userName,
      email: formData.email,
      role: formData.role,
      password: formData.passwords.password
    };
  
    const response = { success: true };
    (authService.register as jasmine.Spy).and.returnValue(of(response));
    component.onSubmit();
  
    expect(authService.register).not.toHaveBeenCalledWith(registerData);
  });

  it('not should register with bad userName', () => {
    const formData = {
      birthday: '01/01/2000',
      userName: '@!%@r',
      email: 'test@example.com',
      role: 'user',
      passwords: {
        password: 'password',
        confirmPassword: 'password'
      }
    };
    component.regForm.setValue(formData);
    component.regForm.updateValueAndValidity();
    
    const registerData: RegisterData = {
      birthday: formData.birthday,
      userName: formData.userName,
      email: formData.email,
      role: formData.role,
      password: formData.passwords.password
    };
  
    const response = { success: true };
    (authService.register as jasmine.Spy).and.returnValue(of(response));
    component.onSubmit();
  
    expect(authService.register).toHaveBeenCalledWith(registerData);
  });
});