import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/pages/home/home.component';
import { LoginComponent } from './components/pages/login/login.component';
import { RegisterComponent } from './components/pages/register/register.component';
import { CabinetComponent } from './components/pages/cabinet/cabinet.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ValidatorMessageComponent } from './components/UI/validator-message/validator-message.component';
import { AUTH_API_URL, RESOURSE_API_URL } from './app-injection-tokens';
import { environment } from 'src/environments/environment';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/auth.service';
import { AdminPanelComponent } from './components/pages/admin-panel/admin-panel.component';
import { StartPageComponent } from './components/pages/start-page/start-page.component';
import { FooterComponent } from './components/footer/footer.component';
import { LectureBlockComponent} from './components/UI/lecture-block/lecture-block.component';
import { CourseBlockComponent } from './components/UI/course-block/course-block.component';
import { ErrorComponent } from './components/pages/error/error.component';
import { NotificationComponent } from './components/UI/notification/notification.component';
import { ModalDialogComponent } from './components/UI/modal-dialog/modal-dialog.component';

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
    StartPageComponent,
    FooterComponent,
    LectureBlockComponent,
    CourseBlockComponent,
    ErrorComponent,
    NotificationComponent,
    ModalDialogComponent
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
    provide: RESOURSE_API_URL,
    useValue: environment.resourseApiUrl
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
