import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';

export function futureDateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) return null;

    const dateValue = new Date(value);
    const now = new Date();

    return dateValue > now ? null : { notInFuture: true };
  };
}
