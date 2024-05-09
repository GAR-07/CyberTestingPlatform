import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/pages/home/home.component';
import { LoginComponent } from './components/pages/login/login.component';
import { RegisterComponent } from './components/pages/register/register.component';
import { CabinetComponent } from './components/pages/cabinet/cabinet.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ValidatorMessageComponent } from './components/UI/validator-message/validator-message.component';
import { USER_API_URL, RESOURSE_API_URL } from './app-injection-tokens';
import { environment } from 'src/environments/environment';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/auth.service';
import { AdminPanelComponent } from './components/pages/admin-panel/admin-panel.component';
import { StartPageComponent } from './components/pages/start-page/start-page.component';
import { FooterComponent } from './components/footer/footer.component';
import { LectureBlockComponent} from './components/lecture-block/lecture-block.component';
import { CourseBlockComponent } from './components/course-block/course-block.component';
import { NotificationComponent } from './components/UI/notification/notification.component';
import { ModalDialogComponent } from './components/UI/modal-dialog/modal-dialog.component';
import { TestBlockComponent } from './components/test-block/test-block.component';
import { TestResultBlockComponent } from './components/test-result-block/test-result-block.component';
import { CourseFormComponent } from './components/course-form/course-form.component';
import { LectureFormComponent } from './components/lecture-form/lecture-form.component';
import { TestFormComponent } from './components/test-form/test-form.component';

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
    CourseBlockComponent,
    CourseFormComponent,
    LectureBlockComponent,
    TestBlockComponent,
    TestResultBlockComponent,
    NotificationComponent,
    ModalDialogComponent,
    LectureFormComponent,
    TestFormComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,

    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.tokenWhiteListedDomains
      }
    })
  ],
  providers: [{
    provide: USER_API_URL,
    useValue: environment.userApiUrl
  },
  {
    provide: RESOURSE_API_URL,
    useValue: environment.resourseApiUrl
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
