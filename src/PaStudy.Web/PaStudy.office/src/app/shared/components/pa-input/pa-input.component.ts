import { Component } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

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
export class PaInputComponent {}
