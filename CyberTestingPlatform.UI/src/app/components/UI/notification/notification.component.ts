import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NotificationMessage } from 'src/app/interfaces/notificationMessage.model';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent {
  @Input() notificationMessage!: NotificationMessage;
	@Output() isConfirmed: EventEmitter<boolean> = new EventEmitter<boolean>();

	ngOnInit(): void {
    if (this.notificationMessage.message === undefined) {
      this.notificationMessage.message = 'Нарушение связи с сервером'
    }
    
    setTimeout(() => {
      this.close();
    }, 5000);
  }
	
	close() {
		this.isConfirmed.emit(true);
	}
}
