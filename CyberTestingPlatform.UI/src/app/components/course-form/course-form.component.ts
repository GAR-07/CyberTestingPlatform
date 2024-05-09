import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-course-form',
  templateUrl: './course-form.component.html',
  styleUrls: ['./course-form.component.scss']
})
export class CourseFormComponent {
  @Input() mode: string = 'list';
  @Input() course!: CourseData;

  roles: string[] = [];
  progress: number = 0;
  dragAreaClass: string = 'dragarea';
  isModalDialogVisible: boolean = false;
  courseForm: FormGroup = this.formBuilder.group({
    name: [null, [
      Validators.required,
      Validators.maxLength(255), 
    ]],
    description: [null, [
      Validators.required,
    ]],
    price: [0, [
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
  ) {}

  ngOnInit(): void {
    var accountData = this.authService.accountData();
    this.roles = accountData ? accountData.role : [];
    this.changeMode(this.mode);
  }

  async changeMode(mode: string) {
    this.mode = mode;
    this.getComponentData();
  }

  async getComponentData() {
    if(this.mode === 'edit') {
      this.courseForm.patchValue({
        name: this.course.name,
        description: this.course.description,
        price: this.course.price,
        imagePath: this.course.imagePath,
      });
    }
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
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
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
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
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
        this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
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
            this.courseForm.patchValue({ imagePath: event.body.filePath });
          } else {
            this.notificationService.addMessage(new NotificationMessage('Ошибка загрузки файла', 500));
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
