import { Component, ElementRef } from '@angular/core';
import { NotificationService } from './services/notification.service';
import { NotificationMessage } from './interfaces/notificationMessage.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'CyberTestingPlatform';
  notificationMessages: NotificationMessage[] = [];

  constructor(
    private notificationService: NotificationService,
    private elem: ElementRef
  ) { }

  ngOnInit(): void {
    this.notificationService.notificationMessages$.subscribe(messages => {
      this.notificationMessages = messages;
    });

    var fontSize = localStorage.getItem('fontSize');
    const htmlElement = this.elem.nativeElement.ownerDocument.documentElement;
    htmlElement.style.fontSize = fontSize + 'px';
  }

  closeNotification(notificationMessage: NotificationMessage) {
    this.notificationService.clearMessage(notificationMessage);
  }
}
