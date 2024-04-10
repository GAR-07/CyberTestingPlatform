import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-course-block',
  templateUrl: './course-block.component.html',
  styleUrls: ['./course-block.component.scss']
})
export class CourseBlockComponent {
  @Input() mods: string[] = [];
  @Input() roles: string[] = [];
  @Input() course!: CourseData;

  mode: string = '';
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
    private formBuilder: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.changeMode(this.mode);
  }

  changeMode(mode: string) {
    this.mode = this.mods.includes(mode) ? mode : this.mods[0];
    
    if(this.mode === 'edit') {
      this.courseForm.patchValue({
        name: this.course.name,
        description: this.course.description,
        price: this.course.price,
        imagePath: this.course.imagePath,
      });
    } else {
      this.courseForm.patchValue({
        name: null,
        description: null,
        price: null,
        imagePath: null,
      });
    }
  }

  createCourse() {
    const date = new Date();
    const convertedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

    this.course = {
      id: '',
      name: this.courseForm.value.name,
      description: this.courseForm.value.description,
      price: this.courseForm.value.price,
      imagePath: this.courseForm.value.imagePath,
      creatorID: this.authService.accountData().sub,
      creationDate: convertedDate,
      lastUpdationDate: '',
    };
    
    this.storageService.createCourse(this.course).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }

  editCourse() {
    const date = new Date();
    const convertedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

    this.course = {
      id: this.course.id,
      name: this.courseForm.value.name,
      description: this.courseForm.value.description,
      price: this.courseForm.value.price,
      imagePath: this.courseForm.value.imagePath,
      creatorID: this.course.creatorID,
      creationDate: this.course.creationDate,
      lastUpdationDate: convertedDate,
    };
    
    this.storageService.updateCourse(this.course.id, this.course).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        console.log(response);
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
        console.log(response);
      }
    });
  }

  uploadFile = (files : any) => {
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
            console.log(event.body.error);
          } else {
            this.courseForm.patchValue({ imagePath: event.body.filePath });
          }
        }
      },
      error: (response: any) => console.log(response)
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

  createFilePath = (serverPath: string) => { 
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
