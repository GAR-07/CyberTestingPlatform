import { HttpEventType } from '@angular/common/http';
import { Component, HostListener, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';
import { convertImgPathToTag, convertTagToImgPath } from 'src/app/utils/conversion-helper';
import { ActivatedRoute } from '@angular/router';
import { TestData } from 'src/app/interfaces/testData.model';
import { NotificationService } from 'src/app/services/notification.service';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';

@Component({
  selector: 'app-lecture-block',
  templateUrl: './lecture-block.component.html',
  styleUrls: ['./lecture-block.component.scss'],
})
export class LectureBlockComponent {

  @Input() mods: string[] = ['view', 'edit'];
  @Input() lecture!: LectureData;
  @Input() course!: CourseData;

  mode: string = '';
  roles: string[] = [];
  lectures!: LectureData[];
  tests!: TestData[];
  courses!: CourseData[];
  progress: number = 0;
  imagePath: string = '';
  currentGuid: string | null = null;
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
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    var accountData = this.authService.accountData();
    this.roles = accountData ? accountData.role : [];
    this.changeMode(this.mode);
    this.getComponentData();

    this.route.paramMap.subscribe(params => {
      const newGuid = params.get('guid');

      if (newGuid !== this.currentGuid) {
        this.currentGuid = newGuid;

        if (newGuid !== null) {
          this.updateComponentData();
          console.log('update lecture!');
        }
      }
    });
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
    }
  }

  async getComponentData() {
    if (this.mode !== 'create') {
      if (!this.lecture) {
        this.currentGuid = this.route.snapshot.paramMap.get('guid');
        await this.getLecture(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);
      }
  
      if (!this.course && this.lecture.courseId !== this.guidEmpty) {
        await this.getCourse(this.lecture.courseId);
      }

      if (this.mode === 'view') {
        await this.getCourseLectures(this.lecture.courseId);
        await this.getCourseTests(this.lecture.courseId);
        
        this.lecture.content = convertImgPathToTag(environment.resourseApiUrl, this.lecture.content);
      } else {
        this.lecture.content = convertTagToImgPath(environment.resourseApiUrl, this.lecture.content);
      }
    }
  }

  async updateComponentData() {
    this.currentGuid = this.route.snapshot.paramMap.get('guid');
    await this.getLecture(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);

    if (this.course.id !== this.lecture.courseId) {
      await this.getCourse(this.lecture.courseId);
    }

    if(this.mode !== 'create' && this.mode !== 'card') {
      await this.getCourseLectures(this.lecture.courseId);
      await this.getCourseTests(this.lecture.courseId);
    }
    
    if (this.mode !== 'create') {
      if (this.mode === 'view') {
        this.lecture.content = convertImgPathToTag(environment.resourseApiUrl, this.lecture.content);
      } else {
        this.lecture.content = convertTagToImgPath(environment.resourseApiUrl, this.lecture.content);
      }
    }
  }

  getLecture(id: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getLecture(id).subscribe({
        next: (response: LectureData) => {
          this.lecture = response;
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
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
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
        console.log(response);
      },
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
    });
  }

  deleteLecture(id: string) {
    this.storageService.deleteLecture(id).subscribe({
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
      error: (response) => {
        this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
      }
    });
  }

  getNextLectureId(id: string) {
    const currentIndex = this.lectures.findIndex(lecture => lecture.id === id);

    if (currentIndex !== -1) {
        const nextIndex = currentIndex + 1;

        if (nextIndex < this.lectures.length) {
            const nextLectureId = this.lectures[nextIndex].id;
            return nextLectureId;
        }
    }
    return '';
  }

  getPreviousLectureId(id: string) {
    const currentIndex = this.lectures.findIndex(lecture => lecture.id === id);

    if (currentIndex !== -1) {
        const previousIndex = currentIndex - 1;

        if (previousIndex >= 0) {
            const previousLectureId = this.lectures[previousIndex].id;
            return previousLectureId;
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