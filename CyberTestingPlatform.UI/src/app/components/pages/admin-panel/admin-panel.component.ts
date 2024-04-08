import { Component, ElementRef, QueryList, Renderer2, ViewChildren} from '@angular/core';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { LectureData } from 'src/app/interfaces/lectureData.model';
import { TestData } from 'src/app/interfaces/testData.model';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent {
  @ViewChildren('tabLinks') tabLinks!: QueryList<ElementRef>;
  @ViewChildren('tabContents') tabContents!: QueryList<ElementRef>;

  courses: CourseData[] = [];
  lectures: LectureData[] = [];
  tests: TestData[] = [];
  pageSize: number = 20;
  contentMods: string[] = ['list', 'view', 'create', 'edit']

  constructor(
    private storageService: StorageService,
    private renderer: Renderer2,
  ) {}

  ngOnInit(): void {
    this.getCourses(1);
    this.getLectures(1);
    this.getTests(1);
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

  getTests(pageNum: number) {
    this.tests = [];
    this.storageService.getTests(this.pageSize, pageNum)
    .subscribe({
      next: (response: TestData[]) => {
        if (response) {
          for (var i = 0; i < response.length; i++) {
            this.tests.push(response[i]);
          }
        }
      },
      error: (response) => console.log(response)
    });
  }

  openTab(tabName: string): void {
    var open = true;
    const nestedElements = this.tabContents.first.nativeElement.querySelectorAll('.tab-content');
    nestedElements.forEach((el: HTMLElement) => {
      if (el.id === tabName) {
        if (el.style.display === 'block') {
          open = false;
          this.renderer.setStyle(el, 'display', 'none');
        } else {
          this.renderer.setStyle(el, 'display', 'block');
        }
      } else {
        this.renderer.setStyle(el, 'display', 'none');
      }
    });

    const buttons = this.tabLinks.first.nativeElement.querySelectorAll('.tab-link');
    buttons.forEach((button: HTMLElement) => {
      if (button.getAttribute('data-tab') === tabName && open) {
        this.renderer.addClass(button, 'primary');
      } else {
        this.renderer.removeClass(button, 'primary');
      }
    });
  }
}
