import { Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoaderService } from '../../../shared/services/loader.service';
import { CourseService } from '../course.service';
import { BaseFilter } from '../../../shared/models/base/base-filter-model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, finalize, take } from 'rxjs';
import { TeacherGradebookDto } from '../models/teacher-gradebook';
import { gradebookTableConfig } from './gradebook-table.config';
import { TableModule } from '../../../shared/components/table/table.module';
import { DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-student-gradebook',
  standalone: true,
  imports: [TableModule, DatePipe, DecimalPipe],
  templateUrl: './student-gradebook.component.html',
  styleUrl: './student-gradebook.component.scss',
})
export class StudentGradebookComponent {
  private destroyRef = inject(DestroyRef);
  private loaderService = inject(LoaderService);
  private courseService = inject(CourseService);
  private route = inject(ActivatedRoute);
  public gradebookData = signal<TeacherGradebookDto[]>([]);
  public courseId!: number;
  public tableConfig = gradebookTableConfig;
  ngOnInit(): void {
    this.defineCourseId();
  }
  private defineCourseId() {
    const courseId = this.route.snapshot.paramMap.get('id');
    if (!courseId) {
      return;
    }
    this.courseId = +courseId;
  }

  fetchGradebook(filter: BaseFilter) {
    this.loaderService.busy();

    this.courseService
      .fetchGradebook(this.courseId)
      .pipe(
        tap((data) => {
          this.gradebookData.set(data);
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
