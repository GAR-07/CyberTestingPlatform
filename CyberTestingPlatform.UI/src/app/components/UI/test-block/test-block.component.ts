import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { TestResultData } from 'src/app/interfaces/testResultData.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';
import { convertImgPathToTag, convertTagToImgPath } from 'src/app/utils/conversion-helper';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-block',
  templateUrl: './test-block.component.html',
  styleUrls: ['./test-block.component.scss']
})
export class TestBlockComponent {

  @Input() mods: string[] = ['view', 'edit'];
  @Input() test!: TestData;
  @Input() course!: CourseData;

  mode: string = '';
  roles: string[] = [];
  lectures!: LectureData[];
  tests!: TestData[];
  courses!: CourseData[];
  questions: string[] = [];
  answerOptions: string[] = [];
  correctAnswers: string[] = [];
  currentQuestion: number = 0;
  testResultId: string | null = null;
  progress: number = 0;
  imagePath: string = '';
  currentGuid: string | null = null;
  dragAreaClass: string = 'dragarea';
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';
  testForm: FormGroup = this.formBuilder.group({
    courseId: [this.guidEmpty, [
      Validators.maxLength(255), 
    ]],
    theme: [null, [
      Validators.required,
      Validators.maxLength(255), 
    ]],
    title: [null, [
      Validators.required,
      Validators.maxLength(255),
    ]],
    questions: [null, [
      Validators.required,
    ]],
    answerOptions: [null, [
      Validators.required,
    ]],
    correctAnswers: [null, [
      Validators.required,
    ]],
    position: [null, [
      Validators.required,
      Validators.min(0),
      Validators.max(100),
    ]]
  })

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  async ngOnInit(): Promise<void> {
    var accountData = this.authService.accountData();
    this.roles = accountData ? accountData.role : [];
    this.changeMode(this.mode);
    
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

  async changeMode(mode: string) {
    this.mode = this.mods.includes(mode) ? mode : this.mods[0];

    if(this.mode === 'edit') {
      await this.getAllCourses();
      if(this.course) {
        this.testForm.patchValue({
          courseId: this.course.id,
        });
      }
      this.testForm.patchValue({
        theme: this.test.theme,
        title: this.test.title,
        questions: this.test.questions,
        answerOptions: this.test.answerOptions,
        correctAnswers: this.test.correctAnswers,
        position: this.test.position,
        courseId: this.test.courseId
      });
    }
  }

  async getComponentData() {
    if (this.mode !== 'create') {
      if (!this.test) {
        this.currentGuid = this.route.snapshot.paramMap.get('guid');
        await this.getTest(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);
      }
      await this.getAdditionalComponentData();
    }
  }

  async updateComponentData() {
    if (this.mode !== 'create') {
      this.currentGuid = this.route.snapshot.paramMap.get('guid');
      await this.getTest(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);
      
      await this.getAdditionalComponentData();
    }
  }

  async getAdditionalComponentData() {
    if (!this.course && this.test.courseId !== this.guidEmpty) {
      await this.getCourse(this.test.courseId);
    }

    if (this.mode === 'view') {
      await this.getCourseLectures(this.test.courseId);
      await this.getCourseTests(this.test.courseId);
      
      for (let i = 0; i < this.questions.length; i++) {
        this.questions[i] = convertImgPathToTag(environment.resourseApiUrl, this.questions[i]);
      }
    } else {
      for (let i = 0; i < this.questions.length; i++) {
        this.questions[i] = convertTagToImgPath(environment.resourseApiUrl, this.questions[i]);
      }
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
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
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
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
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
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
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
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
          reject();
        }
      });
    });
  }

  getAllCourses() {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getAllCourses().subscribe({
        next: (response: CourseData[]) => {
          this.courses = response;
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
          reject();
        }
      });
    });
  }

  createTest() {
    var accountData = this.authService.accountData();
    this.test = {
      id: '',
      theme: this.testForm.value.theme,
      title: this.testForm.value.title,
      questions: this.testForm.value.questions,
      answerOptions: this.testForm.value.answerOptions,
      correctAnswers: this.testForm.value.correctAnswers,
      position: this.testForm.value.position,
      creatorId: accountData ? accountData.sub : '',
      creationDate: '',
      lastUpdationDate: '',
      courseId: this.testForm.value.courseId,
    };
    
    this.storageService.createTest(this.test).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
    });
  }

  editTest() {
    this.test = {
      id: this.test.id,
      theme: this.testForm.value.theme,
      title: this.testForm.value.title,
      questions: this.testForm.value.questions,
      answerOptions: this.testForm.value.answerOptions,
      correctAnswers: this.testForm.value.correctAnswers,
      position: this.testForm.value.position,
      creatorId: this.test.creatorId,
      creationDate: this.test.creationDate,
      lastUpdationDate: '',
      courseId: this.testForm.value.courseId,
    };
    
    this.storageService.updateTest(this.test.id, this.test).subscribe({
      next: (response: any) => {
        this.mode = "";
        console.log(response);
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
    });
  }

  deleteTest(id: string) {
    this.storageService.deleteTest(id).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response: any) => {
        console.log(response.error.Message);
      }
    });
  }

  uploadFile = (files : any) => {
    if (files.length === 0) {
      return;
    }
    
    const formData = new FormData();

    for(var i = 0; i < files.length; i++) {
      var fileToUpload = <File>files[i];
      // Здесь можно добавить проверки файла, например:
      // if (fileToUpload.size > 100000000) {
      //   this.message = 'Файл '+ fileToUpload.name +' слишком большой!';
      //   return;
      // }
      formData.append('Image', fileToUpload, fileToUpload.name)
    }

    this.storageService.uploadFiles(formData)
    .subscribe({
      next: (event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total);
          console.log(this.progress);
        } else if (event.type === HttpEventType.Response) {
          if (event.body.error) {
            console.log(event.body.error);
          } else {
            this.imagePath = event.body.filePath;
          }
        }
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
    });
  }

  saveTestResult() {
    var answers = this.correctAnswers.join('\n');
    var accountData = this.authService.accountData();

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
        this.notificationService.addMessage(new NotificationMessage('success', 'Результаты теста сохранены'));
        this.testResultId = id;
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
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
      this.deleteTest(this.test.id);
    }
	}

  createFilePath(serverPath: string) { 
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }

  @HostListener("dragover", ["$event"]) onDragOver(event: any) {
    this.dragAreaClass = "droparea";
    event.preventDefault();
  }
  @HostListener("dragenter", ["$event"]) onDragEnter(event: any) {
    this.dragAreaClass = "droparea";
    event.preventDefault();
  }
  @HostListener("dragend", ["$event"]) onDragEnd(event: any) {
    this.dragAreaClass = "dragarea";
    event.preventDefault();
  }
  @HostListener("dragleave", ["$event"]) onDragLeave(event: any) {
    this.dragAreaClass = "dragarea";
    event.preventDefault();
  }
  @HostListener("drop", ["$event"]) onDrop(event: any) {
    this.dragAreaClass = "dragarea";
    event.preventDefault();
    event.stopPropagation();
    if (event.dataTransfer.files) {
      let files: FileList = event.dataTransfer.files;
      this.uploadFile(files);
    }
  }
}