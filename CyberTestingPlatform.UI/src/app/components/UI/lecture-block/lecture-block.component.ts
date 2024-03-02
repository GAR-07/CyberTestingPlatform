import { Component, Input} from '@angular/core';
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
      this.getCourseById(this.lecture.courseId)
      console.log(this.courses);
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
        courseId: this.lecture.courseId
      });

    } else {
      this.lectureForm.patchValue({
        theme: null,
        title: null,
        content: null,
        courseId: null
      });
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
      creatorId: this.authService.accountData().sub,
      creationDate: convertedDate,
      lastUpdationDate: '',
      courseId: this.lectureForm.value.courseId,
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
    const convertedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

    this.lecture = {
      id: this.lecture.id,
      theme: this.lectureForm.value.theme,
      title: this.lectureForm.value.title,
      content: this.lectureForm.value.content,
      creatorId: this.lecture.creatorId,
      creationDate: this.lecture.creationDate,
      lastUpdationDate: convertedDate,
      courseId: this.lectureForm.value.courseId,
    };
    
    this.storageService.updateLecture(this.lecture.id, this.lecture).subscribe({
      next: (response: any) => {
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
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });
  }

  showModal() {
		this.isModalDialogVisible = true;
	}

  closeModal(isConfirmed: boolean) {
		this.isModalDialogVisible = false;
    if (isConfirmed === true) {
      this.deleteLecture(this.course.id);
    }
	}

  getCourseById(id: string) {
    this.courses.forEach(course => {
      if (course.id === id) {
        this.course = course;
      }
    });
  }
}
