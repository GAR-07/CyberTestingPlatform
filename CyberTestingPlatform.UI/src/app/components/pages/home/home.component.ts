import { Component } from '@angular/core';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  courses: CourseData[] = [];
  lectures: LectureData[] = [];
  tests!: TestData[];
  pageNum: number = 1;
  pageSize: number = 20;
  contentMods: string[] = ['list', 'view', 'edit']

  constructor(
    private storageService: StorageService,
  ) { }

  async ngOnInit(): Promise<void> {
    try {
      await this.getCourses(this.pageNum);
      await this.getLectures(this.pageNum);
      await this.getTests(this.pageNum);
    } catch (error) {
      console.error('Ошибка загрузки курсов:', error);
    }
  }

  getCourses(pageNum: number): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.courses = [];
      this.storageService.getCourses(this.pageSize, pageNum)
      .subscribe({
        next: (response: CourseData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.courses.push(response[i]);
            }
          }
          resolve();
        },
        error: (error) => {
          console.log(error);
          reject(error);
        }
      });
    });
  }

  getLectures(pageNum: number) {
    return new Promise<void>((resolve, reject) => {
      this.lectures = [];
      this.storageService.getLectures(this.pageSize, pageNum)
      .subscribe({
        next: (response: LectureData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.lectures.push(response[i]);
            }
          }
          resolve();
        },
        error: (error) => {
          console.log(error);
          reject(error);
        }
      });
    });
  }

  getTests(pageNum: number) {
    return new Promise<void>((resolve, reject) => {
      this.tests = [];
      this.storageService.getTests(this.pageSize, pageNum)
      .subscribe({
        next: (response: TestData[]) => {
          if (response) {
            for (var i = 0; i < response.length; i++) {
              this.tests.push(response[i]);
            }
          }
          resolve();
        },
        error: (error) => {
          console.log(error);
          reject(error);
        }
      });
    });
  }
}
