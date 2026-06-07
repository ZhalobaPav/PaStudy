import {
  Component,
  DestroyRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { Note } from '../models/note';
import { TableComponent } from '../../../shared/components/table/table.component';
import { SubmissionFilter } from '../assignments/models/submission';
import { LoaderService } from '../../../shared/services/loader.service';
import { CourseService } from '../course.service';
import { CoursesFilter } from '../models/courses-filter';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap, finalize, take } from 'rxjs';
import { TableModule } from '../../../shared/components/table/table.module';
import { tableConfig } from './note-table.config';
import { BaseFilter } from '../../../shared/models/base/base-filter-model';
import {
  ActivatedRoute,
  RouterLink,
  RouterStateSnapshot,
} from '@angular/router';

@Component({
  selector: 'app-course-notes',
  standalone: true,
  imports: [TableModule, RouterLink],
  templateUrl: './course-notes.component.html',
  styleUrl: './course-notes.component.scss',
})
export class CourseNotesComponent implements OnInit {
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
  @ViewChild(TableComponent, { static: true })
  tableComponentRef!: TableComponent<SubmissionFilter>;
  private destroyRef = inject(DestroyRef);
  private loaderService = inject(LoaderService);
  private courseService = inject(CourseService);
  private route = inject(ActivatedRoute);
  public notes = signal<Note[]>([]);
  public courseId!: number;
  public tableConfig = tableConfig;
  fetchNotes(noteFilter: BaseFilter) {
    this.loaderService.busy();
    this.courseService
      .getNotes(noteFilter, this.courseId)
      .pipe(
        tap((notes) => {
          this.notes.set(notes);
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
