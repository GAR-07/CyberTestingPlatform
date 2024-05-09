import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-course-block',
  templateUrl: './course-block.component.html',
  styleUrls: ['./course-block.component.scss']
})
export class CourseBlockComponent {
  @Input() course!: CourseData;

  lectures!: LectureData[];
  tests!: TestData[];
  groupedLectures: { [key: string]: LectureData[] } = {};
  groupedTests: { [key: string]: TestData[] } = {};
  groupedLecturesArray: { theme: string, lectures: LectureData[] }[] = [];
  groupedTestsArray: { theme: string, tests: TestData[] }[] = [];
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.getComponentData();
  }

  async getComponentData() {
    if (!this.course) {
      var guid = this.route.snapshot.paramMap.get('guid');
      await this.getCourse(guid !== null ? guid : '');
    }

    await this.getCourseLectures(this.course.id);
    await this.getCourseTests(this.course.id);

    this.lectures.sort((a, b) => a.position - b.position);
    this.tests.sort((a, b) => a.position - b.position);

    this.groupedLectures = this.groupItemsByTheme(this.lectures);
    this.groupedLecturesArray = Object.keys(this.groupedLectures).map(theme => ({
      theme: theme,
      lectures: this.groupedLectures[theme]
    }));
    this.groupedTests = this.groupItemsByTheme(this.tests);
    this.groupedTestsArray = Object.keys(this.groupedTests).map(theme => ({
      theme: theme,
      tests: this.groupedTests[theme]
    }));
  }

  getCourse(id: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getCourse(id).subscribe({
        next: (response: CourseData) => {
          this.course = response;          
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  getCourseLectures(courseId: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getLecturesByCourseId(courseId).subscribe({
        next: (response: LectureData[]) => {
          response.sort((a, b) => a.position - b.position);
          this.lectures = response;
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  getCourseTests(courseId: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getTestsByCourseId(courseId).subscribe({
        next: (response: TestData[]) => {
          response.sort((a, b) => a.position - b.position); 
          this.tests = response;
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  groupItemsByTheme(items: any[]): { [key: string]: any[] } {
    const groupedItems: { [key: string]: any[] } = {};

    items.forEach(item => {
      if (!groupedItems[item.theme]) {
        groupedItems[item.theme] = [];
      }
      groupedItems[item.theme].push(item);
    });

    return groupedItems;
  }

  showModal() {
		this.isModalDialogVisible = true;
	}

  closeModal(isConfirmed: boolean) {
		this.isModalDialogVisible = false;
    if (isConfirmed === true) {

    }
	}

  createFilePath(serverPath: string) {
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }
}
