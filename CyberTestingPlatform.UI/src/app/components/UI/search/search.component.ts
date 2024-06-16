import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
  @Input() requestText!: string;
  searchControl: FormControl = new FormControl('');
	@Output() searchChanged: EventEmitter<string> = new EventEmitter<string>();

  ngOnInit() {
    this.searchControl.valueChanges.pipe(
      debounceTime(1000) // задержка для улучшения производительности
    ).subscribe(value => {
      this.searchChanged.emit(value);
    });
  }
}
