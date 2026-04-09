import { Component, input } from '@angular/core';
import { ICourse } from '../../../shared/models/course';

@Component({
  selector: 'app-enrolled-course-element',
  standalone: true,
  imports: [],
  templateUrl: './enrolled-course-element.component.html',
  styleUrl: './enrolled-course-element.component.scss',
})
export class EnrolledCourseElementComponent {
  public course = input.required<ICourse>();
}
