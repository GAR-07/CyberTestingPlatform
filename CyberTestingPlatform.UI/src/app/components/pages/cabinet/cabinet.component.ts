import { Component, ElementRef } from '@angular/core';
import { AccountData } from 'src/app/interfaces/accountData.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-cabinet',
  templateUrl: './cabinet.component.html',
  styleUrls: ['./cabinet.component.scss']
})
export class CabinetComponent {

  currentFontSize: number = 0;
  account!: AccountData;

  constructor(
    private elem: ElementRef,
    private accountService: AccountService,
  ) {}

  ngOnInit(): void {
    const htmlElement = this.elem.nativeElement.ownerDocument.documentElement;
    this.currentFontSize = parseInt(window.getComputedStyle(htmlElement).fontSize, 10);
    this.getComponentData();
  }

  async getComponentData() {
    await this.getAccountData();
  }

  getAccountData() {
    return new Promise<void>((resolve, reject) => {
      this.accountService.getAccountData(this.authService.accountData().sub).subscribe({
        next: (response: CourseData) => {
          this.course = response;          
          resolve();
        },
        error: (response) => {
          this.notificationService.addMessage(new NotificationMessage('error', response.error.Message));
          reject();
        }
      });
    });
  }

  changeFontSize(delta: number): void {
    const htmlElement = this.elem.nativeElement.ownerDocument.documentElement;
    htmlElement.style.fontSize = (this.currentFontSize + delta) + 'px';
    this.currentFontSize = parseInt(window.getComputedStyle(htmlElement).fontSize, 10);
  }

  increaseFontSize(): void {
    this.changeFontSize(2);
  }

  decreaseFontSize(): void {
    this.changeFontSize(-2);
  }
}