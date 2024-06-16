import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/services/notification.service';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';

@Component({
  selector: 'app-lecture-form',
  templateUrl: './lecture-form.component.html',
  styleUrls: ['./lecture-form.component.scss']
})
export class LectureFormComponent {

  @Input() mode: string = 'list';
  @Input() lecture!: LectureData;
  @Input() course!: CourseData;

  roles: string[] = [];
  lectures!: LectureData[];
  courses!: CourseData[];
  progress: number = 0;
  imagePath: string = '';
  dragAreaClass: string = 'dragarea';
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';

  lectureForm: FormGroup = this.formBuilder.group({
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
    content: [null, [
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
    if (this.lecture && this.lecture.courseId !== this.guidEmpty) {
      await this.getCourse(this.lecture.courseId);
    }

    if(this.mode === 'edit') {
      if(this.course) {
        this.lectureForm.patchValue({
          courseId: this.course.id,
        });
      }
      this.lectureForm.patchValue({
        theme: this.lecture.theme,
        title: this.lecture.title,
        content: this.lecture.content,
        position: this.lecture.position,
        courseId: this.lecture.courseId
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
  
  createLecture() {
    const date = new Date();
    const convertedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

    this.lecture = {
      id: '',
      theme: this.lectureForm.value.theme,
      title: this.lectureForm.value.title,
      content: this.lectureForm.value.content,
      position: this.lectureForm.value.position,
      creatorId: this.authService.getAccountData().sub,
      creationDate: convertedDate,
      lastUpdationDate: '',
      courseId: this.lectureForm.value.courseId,
    };
    
    this.storageService.createLecture(this.lecture).subscribe({
      next: (response: any) => {
        window.location.reload();
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
      }
    });
  }

  editLecture() {
    const date = new Date();
    const convertedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

    this.lecture = {
      id: this.lecture.id,
      theme: this.lectureForm.value.theme,
      title: this.lectureForm.value.title,
      content: this.lectureForm.value.content,
      position: this.lectureForm.value.position,
      creatorId: this.lecture.creatorId,
      creationDate: this.lecture.creationDate,
      lastUpdationDate: convertedDate,
      courseId: this.lectureForm.value.courseId,
    };
    
    this.storageService.updateLecture(this.lecture.id, this.lecture).subscribe({
      next: (response: any) => {
        this.mode = "";
        window.location.reload();
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
      }
    });
  }

  deleteLecture(id: string) {
    this.storageService.deleteLecture(id).subscribe({
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
      this.deleteLecture(this.lecture.id);
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
