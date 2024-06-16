import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { TestResultData } from 'src/app/interfaces/testResultData.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';
import { convertImgPathToTag } from 'src/app/utils/conversion-helper';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-block',
  templateUrl: './test-block.component.html',
  styleUrls: ['./test-block.component.scss']
})
export class TestBlockComponent {

  @Input() test!: TestData;
  @Input() course!: CourseData;

  roles: string[] = [];
  lectures!: LectureData[];
  tests!: TestData[];
  questions: string[] = [];
  answerOptions: string[] = [];
  correctAnswers: string[] = [];
  currentQuestion: number = 0;
  testResultId: string | null = null;
  currentGuid: string | null = null;
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  async ngOnInit(): Promise<void> {
    await this.getComponentData();

    this.route.paramMap.subscribe(async params => {
      const newGuid = params.get('guid');

      if (newGuid !== this.currentGuid) {
        this.currentGuid = newGuid;

        if (newGuid !== null) {
          await this.updateComponentData();
        }
      }
    });
  }

  async getComponentData() {
    if (!this.test) {
      this.currentGuid = this.route.snapshot.paramMap.get('guid');
      await this.getTest(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);
    }
    await this.getAdditionalComponentData();
  }

  async updateComponentData() {
    this.currentGuid = this.route.snapshot.paramMap.get('guid');
    await this.getTest(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);
    
    await this.getAdditionalComponentData();
  }

  async getAdditionalComponentData() {
    if (!this.course && this.test.courseId !== this.guidEmpty) {
      await this.getCourse(this.test.courseId);
    }

    await this.getCourseLectures(this.test.courseId);
    await this.getCourseTests(this.test.courseId);
    
    for (let i = 0; i < this.questions.length; i++) {
      this.questions[i] = convertImgPathToTag(environment.resourseApiUrl, this.questions[i]);
    }
  }

  getTest(id: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getTest(id).subscribe({
        next: (response: TestData) => {
          this.test = response;
          this.questions = response.questions.split('\n');
          this.answerOptions = response.answerOptions.split('\n');
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
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

  saveTestResult() {
    var answers = this.correctAnswers.join('\n');
    var accountData = this.authService.getAccountData();

    var testResult = new TestResultData(
      null,
      this.test.id,
      accountData ? accountData.sub : '',
      answers,
      null,
      null
    )

    this.storageService.createTestResult(testResult).subscribe({
      next: (id: string) => {
        this.notificationService.addMessage(new NotificationMessage('Результаты теста сохранены', 200));
        this.testResultId = id;
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
      }
    });
  }

  getTestResults() {
    this.router.navigate(['result/' + this.testResultId]);
  }

  nextQuestion() {
    if (this.currentQuestion < this.questions.length - 1) {
      this.currentQuestion += 1;
    }
  }

  previousQuestion() {
    if (this.currentQuestion >= 1) {
      this.currentQuestion -= 1;
    }
  }

  getNextTestId(id: string) {
    const currentIndex = this.tests.findIndex(test => test.id === id);
    if (currentIndex !== -1) {
      const nextIndex = currentIndex + 1;
      if (nextIndex < this.tests.length) {
        const nextTestId = this.tests[nextIndex].id;
        return nextTestId;
      }
    }
    return '';
  }

  getPreviousTestId(id: string) {
    const currentIndex = this.tests.findIndex(test => test.id === id);
    if (currentIndex !== -1) {
      const previousIndex = currentIndex - 1;
      if (previousIndex >= 0) {
        const previousTestId = this.tests[previousIndex].id;
        return previousTestId;
      }
    }
    return '';
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