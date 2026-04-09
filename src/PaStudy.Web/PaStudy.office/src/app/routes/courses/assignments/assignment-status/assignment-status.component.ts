import { Component, input, signal } from '@angular/core';
import { Assignment } from '../models/assignment-item';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-assignment-status',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './assignment-status.component.html',
  styleUrl: './assignment-status.component.scss',
})
export class AssignmentStatusComponent {
  public assignment = input.required<Assignment>();
}
