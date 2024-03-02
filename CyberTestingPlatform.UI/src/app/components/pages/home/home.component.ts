import { Component } from '@angular/core';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  courses = [];

  constructor(
    private storageService: StorageService,
  ) { }

  ngOnInit(): void { 
    this.storageService.getCourses(5, 0).subscribe({
      next: (response: any) => {
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      }
    });;

  }


}
