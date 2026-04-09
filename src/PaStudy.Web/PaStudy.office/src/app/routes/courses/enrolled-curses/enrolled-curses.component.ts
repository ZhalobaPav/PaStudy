import { Component, inject, OnInit, signal } from '@angular/core';
import { CoursesFilter } from '../models/courses-filter';
import { CourseService } from '../course.service';
import { CourseQuantity } from '../../../shared/enums/course-quantity-type';
import { tap } from 'rxjs';
import { ICourse } from '../../../shared/models/course';

@Component({
  selector: 'app-enrolled-curses',
  standalone: true,
  imports: [],
  templateUrl: './enrolled-curses.component.html',
  styleUrl: './enrolled-curses.component.scss',
})
export class EnrolledCursesComponent implements OnInit {
  private coursesService = inject(CourseService);
  public courses = signal<ICourse[]>([]);
  ngOnInit(): void {
    const filter: CoursesFilter = {
      courseQuantity: CourseQuantity.Enrolled,
    };
    this.fetchCourses(filter);
  }

  private fetchCourses(filters: CoursesFilter) {
    this.coursesService
      .getCourses(filters)
      .pipe(
        tap((response) => {
          this.courses.set(response);
        }),
      )
      .subscribe();
  }
}
