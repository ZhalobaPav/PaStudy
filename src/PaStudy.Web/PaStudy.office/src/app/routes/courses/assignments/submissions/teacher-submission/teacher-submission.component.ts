import {
  Component,
  computed,
  inject,
  input,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { AssignmentService } from '../../assignment.service';
import {
  SUBMISSION_STATUS_LABELS,
  SubmissionFilter,
  SubmissionListItem,
  SubmissionStatus,
} from '../../models/submission';
import { catchError, finalize, of, take, tap } from 'rxjs';
import { TableComponent } from '../../../../../shared/components/table/table.component';
import { CoursesFilter } from '../../../models/courses-filter';
import { TableModule } from '../../../../../shared/components/table/table.module';
import { TableConfig } from '../../../../../shared/components/table/models/table.models';
import { tableConfig } from './submission-table.config';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AssignmentType } from '../../../../../shared/enums/assignment-type';
import { SubmissionStatusPipe } from '../../../../../shared/pipes/submission-status.pipe';

@Component({
  selector: 'app-teacher-submission',
  standalone: true,
  imports: [TableModule, DatePipe, RouterLink, SubmissionStatusPipe],
  templateUrl: './teacher-submission.component.html',
  styleUrl: './teacher-submission.component.scss',
})
export class TeacherSubmissionComponent implements OnInit {
  private assignmentService = inject(AssignmentService);
  public submissions = signal<SubmissionListItem[]>([]);
  public assignmentId = input.required<number>();
  public isLoading = signal(false);
  public page = signal(1);
  public SubmissionStatus = SubmissionStatus;
  public readonly statusLabels = SUBMISSION_STATUS_LABELS;
  public submissionContext!: { row: SubmissionListItem };
  public assignmentType = input.required<AssignmentType | undefined>();
  public isQuiz = computed(() => this.assignmentType() === AssignmentType.Quiz);
  public tableConfig: TableConfig = tableConfig;
  ngOnInit(): void {}
  @ViewChild(TableComponent, { static: true })
  tableComponentRef!: TableComponent<SubmissionFilter>;

  public fetchSubmissions(filter: SubmissionFilter) {
    this.isLoading.set(true);
    this.assignmentService
      .fetchSubmissions(filter, this.assignmentId())
      .pipe(
        tap((response) => {
          this.submissions.set(response);
        }),
        finalize(() => this.isLoading.set(false)),
        catchError((err) => {
          console.error(err);
          return of(err);
        }),
        take(1),
      )
      .subscribe();
  }
}
