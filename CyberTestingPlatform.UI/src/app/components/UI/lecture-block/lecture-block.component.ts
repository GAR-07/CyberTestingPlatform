import { HttpEventType } from '@angular/common/http';
import { Component, Input,} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-lecture-block',
  templateUrl: './lecture-block.component.html',
  styleUrls: ['./lecture-block.component.scss'],
})
export class LectureBlockComponent {

  @Input() mods: string[] = [];
  @Input() courses: CourseData[] = [];
  @Input() lecture!: LectureData;

  mode: string = '';
  course!: CourseData;
  progress: number = 0;
  imagePath: string = '';
  dragAreaClass: string = 'dragarea';
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';
  lectureForm: FormGroup = this.formBuilder.group({
    courseId: [null, [
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
      Validators.min(1),
      Validators.max(100),
    ]]
  }) 

  trustedHtml: any

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private formBuilder: FormBuilder,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.changeMode(this.mode);
    if(this.mode !== 'create') {
      this.getCourseById(this.lecture.courseId)
    }
  }

  changeMode(mode: string) {
    this.mode = this.mods.includes(mode) ? mode : this.mods[0];

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

    } else {
      this.lectureForm.patchValue({
        theme: null,
        title: null,
        content: null,
        position: null,
        courseId: null
      });
    }
    if(this.mode === 'view') {
      if (this.lecture.content) {
        const lines = this.lecture.content.split('\n');
        for (let i = 0; i < lines.length; i++) {
          if (lines[i].includes('Resources\\Images\\')) {
            lines[i] = `<img class="content-image" src="${this.createFilePath(lines[i])}" alt="Изображение">`;
          }
        }
        this.lecture.content = lines.join('<br>');
      }
    }
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
      creatorId: this.authService.accountData().sub,
      creationDate: convertedDate,
      lastUpdationDate: '',
      courseId: this.lectureForm.value.courseId,
    };
    
    this.storageService.createLecture(this.lecture).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        console.log(response);
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
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }

  deleteLecture(id: string) {
    this.storageService.deleteLecture(id).subscribe({
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
            this.imagePath = event.body.filePath;
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
      this.deleteLecture(this.lecture.id);
    }
	}

  getCourseById(id: string) {
    this.courses.forEach(course => {
      if (course.id === id) {
        this.course = course;
      }
    });
  }

  createFilePath = (serverPath: string) => { 
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }
}
