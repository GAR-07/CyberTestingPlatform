import { Component, Input } from '@angular/core';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';
import { convertImgPathToTag} from 'src/app/utils/conversion-helper';
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

  @Input() lecture!: LectureData;
  @Input() course!: CourseData;

  roles: string[] = [];
  lectures!: LectureData[];
  tests!: TestData[];
  currentGuid: string | null = null;
  isModalDialogVisible: boolean = false;
  guidEmpty: string = '00000000-0000-0000-0000-000000000000';

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private route: ActivatedRoute
  ) { }

  async ngOnInit(): Promise<void> {
    await this.getComponentData();

    this.route.paramMap.subscribe(async params => {
      const newGuid = params.get('guid');

      if (newGuid !== this.currentGuid) {
        this.currentGuid = newGuid;

        if (newGuid !== null) {
          await this.updateComponentData();
        }
      }
    });
  }

  async getComponentData() {
    if (!this.lecture) {
      this.currentGuid = this.route.snapshot.paramMap.get('guid');
      await this.getLecture(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);
    }
    await this.getAdditionalComponentData();
  }

  async updateComponentData() {
    this.currentGuid = this.route.snapshot.paramMap.get('guid');
    await this.getLecture(this.currentGuid !== null ? this.currentGuid : this.guidEmpty);
    
    await this.getAdditionalComponentData();
  }

  async getAdditionalComponentData() {
    if (!this.course && this.lecture.courseId !== this.guidEmpty) {
      await this.getCourse(this.lecture.courseId);
    }

    await this.getCourseLectures(this.lecture.courseId);
    await this.getCourseTests(this.lecture.courseId);
    
    this.lecture.content = convertImgPathToTag(environment.resourseApiUrl, this.lecture.content);
  }

  getLecture(id: string) {
    return new Promise<void>((resolve, reject) => {
      this.storageService.getLecture(id).subscribe({
        next: (response: LectureData) => {
          this.lecture = response;
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
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
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
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
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
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
          this.notificationService.addMessage(new NotificationMessage(response.error, response.status));
          reject();
        }
      });
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

    }
	}

  createFilePath(serverPath: string) { 
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }
}