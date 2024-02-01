import { Component, ElementRef } from '@angular/core';

@Component({
  selector: 'app-cabinet',
  templateUrl: './cabinet.component.html',
  styleUrls: ['./cabinet.component.scss']
})
export class CabinetComponent {

  currentFontSize: number = 0;

  constructor(
    private elem: ElementRef
  ) {}

  ngOnInit(): void {
    const htmlElement = this.elem.nativeElement.ownerDocument.documentElement;
    this.currentFontSize = parseInt(window.getComputedStyle(htmlElement).fontSize, 10);
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