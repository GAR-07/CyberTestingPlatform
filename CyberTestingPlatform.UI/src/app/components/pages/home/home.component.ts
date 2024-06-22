import { Component } from '@angular/core';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  courses!: CourseData[];
  lectures!: LectureData[];
  tests!: TestData[];
  pageNum: number = 1;
  pageSize: number = 24;
  searchValue: string | null = null;

  constructor(
    private storageService: StorageService,
    private notificationService: NotificationService,
  ) { }

  async ngOnInit(): Promise<void> {
    await this.getCourses(this.pageNum);
    // await this.getLectures(this.pageNum);
    // await this.getTests(this.pageNum);
  }

  getCourses(pageNum: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.courses = [];
      this.storageService.getCourses(this.searchValue, pageNum, this.pageSize)
      .subscribe({
        next: (response: CourseData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.courses.push(response[i]);
            }
          }
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
          reject();
        }
      });
    });
  }

  getLectures(pageNum: number) {
    return new Promise<void>((resolve, reject) => {
      this.lectures = [];
      this.storageService.getLectures(null, pageNum, this.pageSize)
      .subscribe({
        next: (response: LectureData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.lectures.push(response[i]);
            }
          }
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
          reject();
        }
      });
    });
  }

  getTests(pageNum: number) {
    return new Promise<void>((resolve, reject) => {
      this.tests = [];
      this.storageService.getTests(null, pageNum, this.pageSize)
      .subscribe({
        next: (response: TestData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.tests.push(response[i]);
            }
          }
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
          reject();
        }
      });
    });
  }

  onSearchChanged(searchValue: string) {
    this.searchValue = searchValue;
    this.getCourses(this.pageNum);
    console.log(this.courses);
  }

  createFilePath(serverPath: string) {
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }
}