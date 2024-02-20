import { Component, Input, SimpleChanges} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-lecture-block',
  templateUrl: './lecture-block.component.html',
  styleUrls: ['./lecture-block.component.scss']
})
export class LectureBlockComponent {

  @Input() mods: string[] = [];
  @Input() courses: CourseData[] = [];
  @Input() lecture!: LectureData;


  mode: string = '';
  course!: CourseData;
  lectureForm: FormGroup = this.formBuilder.group({
    courseTheme: [null, [
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
      Validators.minLength(1),
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
      if(this.course) {
        this.lectureForm.patchValue({
          courseTheme: this.course.theme,
        });
      }
      this.lectureForm.patchValue({
        theme: this.lecture.theme,
        title: this.lecture.title,
        content: this.lecture.content,
        courseId: this.lecture.courseId
      });
    } else {
      this.lectureForm.patchValue({
        courseTheme: null,
        theme: null,
        title: null,
        content: null,
        courseId: null
      });
    }

    if (this.mode != 'create') {
      this.getCourseById(this.lecture.courseId)
    }
  }

  createLecture() {
    const date = new Date();
    const creationDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
    this.getCourseByTheme(this.lectureForm.value.courseTheme)

    this.lecture = {
      id: '',
      theme: this.lectureForm.value.theme,
      title: this.lectureForm.value.title,
      content: this.lectureForm.value.content,
      creatorId: this.authService.accountData().sub,
      creationDate: creationDate,
      lastUpdationDate: creationDate,
      courseId: this.course.id,
    };
    
    this.storageService.createLecture(this.lecture).subscribe({
      next: (response: any) => {
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }

  editLecture() {
    const date = new Date();
    const updationDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
    this.getCourseByTheme(this.lectureForm.value.courseTheme)

    this.lecture = {
      id: this.lecture.id,
      theme: this.lectureForm.value.theme,
      title: this.lectureForm.value.title,
      content: this.lectureForm.value.content,
      creatorId: '',
      creationDate: '',
      lastUpdationDate: updationDate,
      courseId: this.course.id,
    };
    
    this.storageService.createLecture(this.lecture).subscribe({
      next: (response: any) => {
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }

  getCourseById(id: string) {
    this.courses.forEach(course => {
      if (course.id === id) {
        this.course = course;
      }
    });
  }

  getCourseByTheme(theme: string) {
    this.courses.forEach(course => {
      if (course.theme === theme) {
        this.course = course;
      }
    });
  }
}
