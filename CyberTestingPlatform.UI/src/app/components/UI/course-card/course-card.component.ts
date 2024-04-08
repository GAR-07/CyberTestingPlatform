import { Component, Input } from '@angular/core';
import { CourseData } from 'src/app/interfaces/courseData.model';
import { StorageService } from 'src/app/services/storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-course-card',
  templateUrl: './course-card.component.html',
  styleUrls: ['./course-card.component.scss']
})
export class CourseCardComponent {
  @Input() orientation: string = 'vertical';
  @Input() course!: CourseData;

  isModalDialogVisible: boolean = false;

  constructor(
    private storageService: StorageService,
  ) { }

  showModal() {
		this.isModalDialogVisible = true;
	}

  closeModal(isConfirmed: boolean) {
		this.isModalDialogVisible = false;
    if (isConfirmed === true) {
      console.log('Успех!');
    }
	}

  createFilePath = (serverPath: string) => { 
    return `${environment.resourseApiUrl}/${serverPath}`; 
  }
}