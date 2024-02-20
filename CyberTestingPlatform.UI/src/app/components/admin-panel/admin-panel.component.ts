import { Component, Input} from '@angular/core';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent {

  courses: CourseData[] = [];
  lectures: LectureData[] = [];
  pageSize: number = 20;
  contentMods: string[] = ['list', 'create', 'edit']
  pageMod: string = 'list';

  constructor(
    private storageService: StorageService,
  ) {}

  ngOnInit(): void {
    this.getCourses(1);
    this.getLectures(1);
  }

  getCourses(pageNum: number) {
    this.courses = [];
    this.storageService.getCourses(this.pageSize, pageNum)
    .subscribe({
      next: (response: CourseData[]) => {
        if (response) {
          for (var i = 0; i < response.length; i++) {
            this.courses.push(response[i]);
          }
        }
      },
      error: (response) => console.log(response)
    });
  }

  getLectures(pageNum: number) {
    this.lectures = [];
    this.storageService.getLectures(this.pageSize, pageNum)
    .subscribe({
      next: (response: LectureData[]) => {
        if (response) {
          for (var i = 0; i < response.length; i++) {
            this.lectures.push(response[i]);
          }
        }
      },
      error: (response) => console.log(response)
    });
  }
}
