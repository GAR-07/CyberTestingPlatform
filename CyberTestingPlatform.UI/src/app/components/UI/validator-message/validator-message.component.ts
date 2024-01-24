import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-validator-message',
  templateUrl: './validator-message.component.html',
  styleUrls: ['./validator-message.component.scss']
})
export class ValidatorMessageComponent {
  @Input() field: AbstractControl | null = null;

  public get validatorMessages() {
    const field = this.field;
    if (!field || !field.errors) {
      return null;
    }

    const errors: string[] = [];
    const config = {
      required: 'Поле является обязательным для заполнения',
      email: 'Поле должно содержать адрес электронной почты',
      pattern: 'Поле не соответствует шаблону ввода',
      custom: '',
      minlength: '',
      maxlength: '',
      min: '',
      max: '',
    };

    if (field.errors.hasOwnProperty('custom')) {
      config['custom'] = typeof field.errors['custom'] === 'string' && field.errors['custom'].length ?
        field.errors['custom'] : 'Не соответствует формату';
    }
    if (field.errors.hasOwnProperty('minlength')) {
      config['minlength'] = `Минимальная длина ${field.errors['minlength'].requiredLength}`;
    }
    if (field.errors.hasOwnProperty('maxlength')) {
      config['maxlength'] = `Максимальная длина ${field.errors['maxlength'].requiredLength}`;
    }
    if (field.errors.hasOwnProperty('min')) {
      config['min'] = `Минимальное значение ${field.errors['min'].min}`;
    }
    if (field.errors.hasOwnProperty('max')) {
      config['max'] = `Максимальное значение ${field.errors['max'].max}`;
    }
    
    Object.keys(field.errors).forEach((error: string) => {
      if ((config as Record<string, string>)[error] !== undefined) {
        errors.push((config as Record<string, string>)[error]);
      } else {
        errors.push(error);
      }
    });

    return errors;
  }
}
