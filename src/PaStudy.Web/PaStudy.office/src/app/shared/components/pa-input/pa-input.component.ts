import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'pa-input',
  standalone: true,
  imports: [],
  templateUrl: './pa-input.component.html',
  styleUrl: './pa-input.component.scss',
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: PaInputComponent, multi: true },
  ],
})
export class PaInputComponent implements ControlValueAccessor {
  public value: any;
  public isDisabled = false;
  writeValue(value: any): void {
    this.value = value;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }
  @Input()
  type!: 'text' | 'number' | 'date';
  @Input() label: string = '';
  @Input() placeholder: string = '';
  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  handleInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
    this.onChange(this.value);
  }

  handleBlur() {
    this.onTouched();
  }
}
