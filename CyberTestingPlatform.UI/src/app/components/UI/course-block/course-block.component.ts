import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  @Input() mods: string[] = ['view', 'edit'];
  @Input() course!: CourseData;

  mode: string = '';
  roles: string[] = [];
  lectures!: LectureData[];
  tests!: TestData[];
  groupedLectures: { [key: string]: LectureData[] } = {};
  groupedTests: { [key: string]: TestData[] } = {};
  groupedLecturesArray: { theme: string, lectures: LectureData[] }[] = [];
  groupedTestsArray: { theme: string, tests: TestData[] }[] = [];
  progress: number = 0;
  dragAreaClass: string = 'dragarea';
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';
  courseForm: FormGroup = this.formBuilder.group({
    name: [null, [
      Validators.required,
      Validators.maxLength(255), 
    ]],
    description: [null, [
      Validators.required,
    ]],
    price: [null, [
      Validators.required,
    ]],
    imagePath: [null, [
      Validators.required,
    ]]
  })

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    var accountData = this.authService.accountData();
    this.roles = accountData ? accountData.role : [];
    this.changeMode(this.mode);
    this.getComponentData();
  }

  async changeMode(mode: string) {
    this.mode = this.mods.includes(mode) ? mode : this.mods[0];

    if(this.mode === 'edit') {
      this.courseForm.patchValue({
        name: this.course.name,
        description: this.course.description,
        price: this.course.price,
        imagePath: this.course.imagePath,
      });
    }
  }

  async getComponentData() {
    if (this.mode !== 'create') {
      if (!this.course) {
        var guid = this.route.snapshot.paramMap.get('guid');
        await this.getCourse(guid !== null ? guid : '');
      }

      if(this.mode !== 'card') {
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

  createCourse() {
    this.course = {
      id: '',
      name: this.courseForm.value.name,
      description: this.courseForm.value.description,
      price: this.courseForm.value.price,
      imagePath: this.courseForm.value.imagePath,
      creatorId: this.authService.accountData().sub,
      creationDate: '',
      lastUpdationDate: '',
    };
    
    this.storageService.createCourse(this.course).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
    });
  }

  editCourse() {
    this.course = {
      id: this.course.id,
      name: this.courseForm.value.name,
      description: this.courseForm.value.description,
      price: this.courseForm.value.price,
      imagePath: this.courseForm.value.imagePath,
      creatorId: this.course.creatorId,
      creationDate: this.course.creationDate,
      lastUpdationDate: '',
    };
    
    this.storageService.updateCourse(this.course.id, this.course).subscribe({
      next: (response: any) => {
        this.mode = "";
        console.log(response);
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
    });
  }

  deleteCourse(id: string) {
    this.storageService.deleteCourse(id).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
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
      // Добавить проверки файла
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
            this.notificationService.addMessage(new NotificationMessage('error', event.body.error.Message));
          } else {
            this.courseForm.patchValue({ imagePath: event.body.filePath });
          }
        }
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
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
      this.deleteCourse(this.course.id);
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
