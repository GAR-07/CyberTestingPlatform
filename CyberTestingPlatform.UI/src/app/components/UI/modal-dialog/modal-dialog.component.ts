import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal-dialog',
  templateUrl: './modal-dialog.component.html',
  styleUrls: ['./modal-dialog.component.scss']
})
export class ModalDialogComponent {
  @Input() header!: string;
	@Input() description!: string;
	@Output() isConfirmed: EventEmitter<boolean> = new EventEmitter<boolean>();

	confirm() {
		this.isConfirmed.emit(true);
	}
	
	close() {
		this.isConfirmed.emit(false);
	}
}