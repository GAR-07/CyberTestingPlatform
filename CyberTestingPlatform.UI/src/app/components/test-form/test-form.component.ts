import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';
import { convertImgPathToTag, convertTagToImgPath } from 'src/app/utils/conversion-helper';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-form',
  templateUrl: './test-form.component.html',
  styleUrls: ['./test-form.component.scss']
})
export class TestFormComponent {

  @Input() mode: string = 'list';
  @Input() test!: TestData;
  @Input() course!: CourseData;

  roles: string[] = [];
  tests!: TestData[];
  courses!: CourseData[];
  progress: number = 0;
  imagePath: string = '';
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
  ) { }

  ngOnInit() {
    var accountData = this.authService.getAccountData();
    this.roles = accountData ? accountData.role : [];
    this.changeMode(this.mode);
  }

  changeMode(mode: string) {
    this.mode = mode;

    this.getComponentData();
  }

  async getComponentData() {
    if (this.test && this.test.courseId !== this.guidEmpty) {
      await this.getCourse(this.test.courseId);
    }

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

  getAllCourses() {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getAllCourses().subscribe({
        next: (response: CourseData[]) => {
          this.courses = response;
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
    });
  }

  createTest() {
    var accountData = this.authService.getAccountData();
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
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
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
        window.location.reload();
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
      }
    });
  }

  deleteTest(id: string) {
    this.storageService.deleteTest(id).subscribe({
      next: (response: any) => {
        window.location.reload();
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
          if (event.body.filePath) {
            this.imagePath = event.body.filePath;
          } else {
            this.notificationService.addMessage(new NotificationMessage('Ошибка загрузки файла', 400));
          }
        }
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
      }
    });
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