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
import { finalize, take, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ICourse } from '../../../shared/models/course';
import { LoaderService } from '../../../shared/services/loader.service';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrl: './courses.component.scss',
  standalone: false,
})
export class CoursesComponent {
  private courseService = inject(CourseService);
  private destroyRef = inject(DestroyRef);
  private loaderService = inject(LoaderService);

  public courses = signal<ICourse[]>([]);
  @ViewChild(TableComponent, { static: true })
  tableComponentRef!: TableComponent<CoursesFilter>;

  fetchCourses(coursesFilter: CoursesFilter) {
    this.loaderService.busy();
    this.courseService
      .getCourses(coursesFilter)
      .pipe(
        tap((courses) => {
          this.courses.set(courses);
        }),
        finalize(() => {
          this.loaderService.idle();
        }),
        take(1),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }
}
