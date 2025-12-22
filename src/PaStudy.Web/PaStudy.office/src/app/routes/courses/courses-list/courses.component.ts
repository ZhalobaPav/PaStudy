import {
  Component,
  DestroyRef,
  inject,
  signal,
  ViewChild,
} from '@angular/core';
import { TableComponent } from '../../../shared/components/table/table.component';
import { CoursesFilter } from '../models/courses-filter';
import { CourseService } from '../course.service';
import { take, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ICourse } from '../../../shared/models/course';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrl: './courses.component.scss',
})
export class CoursesComponent {
  private courseService = inject(CourseService);
  private destroyRef = inject(DestroyRef);

  public courses = signal<ICourse[]>([]);
  @ViewChild(TableComponent, { static: true })
  tableComponentRef!: TableComponent<CoursesFilter>;

  fetchCourses(coursesFilter: CoursesFilter) {
    this.courseService
      .getCourses(coursesFilter)
      .pipe(
        tap((courses) => {
          this.courses.set(courses);
        }),
        take(1),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}
