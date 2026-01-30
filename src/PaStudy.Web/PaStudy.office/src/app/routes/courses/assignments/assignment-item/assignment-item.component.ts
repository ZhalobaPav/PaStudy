import { Component, input } from '@angular/core';
import { Assignment } from '../models/assignment-item';

@Component({
  selector: 'app-assignment-item',
  templateUrl: './assignment-item.component.html',
  styleUrl: './assignment-item.component.scss',
  standalone: false,
})
export class AssignmentItemComponent {
  public assignment = input.required<Assignment | null>();
}
