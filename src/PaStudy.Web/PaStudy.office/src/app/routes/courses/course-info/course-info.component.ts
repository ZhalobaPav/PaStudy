import { Component, inject, input, OnInit, signal } from '@angular/core';
import { ICourse } from '../../../shared/models/course';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from '../course.service';
import { take, tap } from 'rxjs/operators';

@Component({
  selector: 'app-course-info',
  templateUrl: './course-info.component.html',
  styleUrl: './course-info.component.scss',
})
export class CourseInfoComponent {
  public course = input.required<ICourse | null>();
}
