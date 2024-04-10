import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { AdminGuard } from './guards/admin.guard';
import { HomeComponent } from './components/pages/home/home.component';
import { CabinetComponent } from './components/pages/cabinet/cabinet.component';
import { LoginComponent } from './components/pages/login/login.component';
import { RegisterComponent } from './components/pages/register/register.component';
import { AdminPanelComponent } from './components/pages/admin-panel/admin-panel.component';
import { StartPageComponent } from './components/pages/start-page/start-page.component';
import { CourseBlockComponent } from './components/UI/course-block/course-block.component';
import { LectureBlockComponent } from './components/UI/lecture-block/lecture-block.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'start', component: StartPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'cabinet', component: CabinetComponent, canActivate: [AuthGuard] }, 
  { path: 'adminPanel', component: AdminPanelComponent, canActivate: [AdminGuard] },
  { path: 'course/:guid', component: CourseBlockComponent, canActivate: [AuthGuard] },
  { path: 'lecture/:guid', component: LectureBlockComponent, canActivate: [AuthGuard] },
  { path: '**', component: HomeComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
