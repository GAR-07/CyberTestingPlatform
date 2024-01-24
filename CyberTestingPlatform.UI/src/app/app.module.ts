import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CabinetComponent } from './components/cabinet/cabinet.component';
import { NavbarComponent } from './components/UI/navbar/navbar.component';
import { ValidatorMessageComponent } from './components/UI/validator-message/validator-message.component';
import { AUTH_API_URL, STORE_API_URL } from './app-injection-tokens';
import { environment } from 'src/environments/environment';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/auth.service';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { StartPageComponent } from './components/start-page/start-page.component';

export function tokenGetter() {
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    CabinetComponent,
    NavbarComponent,
    ValidatorMessageComponent,
    AdminPanelComponent,
    StartPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,

    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.tokenWhiteListedDomains
      }
    })
  ],
  providers: [{
    provide: AUTH_API_URL,
    useValue: environment.authApiUrl
  },
  {
    provide: STORE_API_URL,
    useValue: environment.storeApiUrl
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
