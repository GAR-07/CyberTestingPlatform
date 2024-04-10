import { HttpEventType } from '@angular/common/http';
import { Component, Input} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-block',
  templateUrl: './test-block.component.html',
  styleUrls: ['./test-block.component.scss']
})
export class TestBlockComponent {

  @Input() mods: string[] = [];
  @Input() roles: string[] = [];
  @Input() courses!: CourseData[];
  @Input() test!: TestData;

  mode: string = '';
  course!: CourseData;
  progress: number = 0;
  imagePath: string = '';
  dragAreaClass: string = 'dragarea';
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';
  testForm: FormGroup = this.formBuilder.group({
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
    questions: [null, [
      Validators.required,
    ]],    
    answerOptions: [null, [
      Validators.required,
    ]],    
    answerCorrect: [null, [
      Validators.required,
    ]],
    position: [null, [
      Validators.required,
      Validators.min(1),
      Validators.max(100),
    ]]
  }) 

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private formBuilder: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.changeMode(this.mode);
    if(this.mode !== 'create') {
      this.getCourseById(this.test.courseId)
      console.log(this.courses);
    }
  }

  changeMode(mode: string) {
    this.mode = this.mods.includes(mode) ? mode : this.mods[0];

    if(this.mode === 'edit') {
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
        answerCorrect: this.test.answerCorrect,
        position: this.test.position,
        courseId: this.test.courseId
      });

    } else {
      this.testForm.patchValue({
        theme: null,
        title: null,
        questions: null,
        answerOptions: null,
        answerCorrect: null,
        courseId: null
      });
    }
  }

  createTest() {
    const date = new Date();
    const convertedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

    this.test = {
      id: '',
      theme: this.testForm.value.theme,
      title: this.testForm.value.title,
      questions: this.testForm.value.questions,
      answerOptions: this.testForm.value.answerOptions,
      answerCorrect: this.testForm.value.answerCorrect,
      position: this.testForm.value.position,
      creatorId: this.authService.accountData().sub,
      creationDate: convertedDate,
      lastUpdationDate: '',
      courseId: this.testForm.value.courseId,
    };
    
    this.storageService.createTest(this.test).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }

  editTest() {
    const date = new Date();
    const convertedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

    this.test = {
      id: this.test.id,
      theme: this.testForm.value.theme,
      title: this.testForm.value.title,
      questions: this.testForm.value.questions,
      answerOptions: this.testForm.value.answerOptions,
      answerCorrect: this.testForm.value.answerCorrect,
      position: this.testForm.value.position,
      creatorId: this.test.creatorId,
      creationDate: this.test.creationDate,
      lastUpdationDate: convertedDate,
      courseId: this.testForm.value.courseId,
    };
    
    this.storageService.updateTest(this.test.id, this.test).subscribe({
      next: (response: any) => {
        window.location.reload();
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }

  deleteTest(id: string) {
    this.storageService.deleteTest(id).subscribe({
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
      this.deleteTest(this.test.id);
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
