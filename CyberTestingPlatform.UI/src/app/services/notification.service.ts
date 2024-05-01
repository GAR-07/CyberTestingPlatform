import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { NotificationMessage } from '../interfaces/notificationMessage.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notificationMessagesSubject: BehaviorSubject<NotificationMessage[]> = new BehaviorSubject<NotificationMessage[]>([]);
  public notificationMessages$: Observable<NotificationMessage[]> = this.notificationMessagesSubject.asObservable();

  constructor() { }

  addMessage(notificationMessages: NotificationMessage) {
    const currentMessages = this.notificationMessagesSubject.getValue();
    const updatedMessages = [...currentMessages, notificationMessages];
    this.notificationMessagesSubject.next(updatedMessages);
  }

  clearMessage(message: NotificationMessage) {
    const currentMessages = this.notificationMessagesSubject.getValue();
    const updatedMessages = currentMessages.filter(msg => msg !== message);
    this.notificationMessagesSubject.next(updatedMessages);
  }
}
