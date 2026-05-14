import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Assignment } from '../models/assignment-item';
import { ActivatedRoute, Router } from '@angular/router';
import { AssignmentService } from '../assignment.service';
import { finalize, take, tap } from 'rxjs';
import { AssignmentType } from '../../../../shared/enums/assignment-type';
import { AuthService } from '../../../auth/auth.service';
import { AssignmentTabType } from '../models/assignment-tab-type';
import { LoaderService } from '../../../../shared/services/loader.service';
import { NotificationService } from '../../../../shared/services/notification.service';

@Component({
  selector: 'app-assignment-details',
  standalone: false,
  templateUrl: './assignment-details.component.html',
  styleUrl: './assignment-details.component.scss',
})
export class AssignmentDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private assignmentService = inject(AssignmentService);
  private authService = inject(AuthService);
  private loaderService = inject(LoaderService);
  public assignment = signal<Assignment | null>(null);
  public readonly currentDate = signal<Date>(new Date());
  private readonly typesWithoutNote = [AssignmentType.Reading];
  public readonly AssignmentType = AssignmentType;
  private toasterService = inject(NotificationService);
  public activeTab = signal<AssignmentTabType>('content');
  public isOverdue = computed(() => {
    const assignment = this.assignment();
    if (!assignment?.dueDate) return false;

    const dueDate = new Date(assignment.dueDate);
    return this.currentDate().getTime() > dueDate.getTime();
  });

  public isSubmitBlocked = computed(() => {
    return !!this.assignment()?.submissionInfo?.isSubmitted;
  });

  public isQuiz = computed(
    () => this.assignment()?.assignmentType === AssignmentType.Quiz,
  );

  public hasNote = computed(
    () => !this.typesWithoutNote.includes(this.assignment()!.assignmentType),
  );

  public isStudent = computed(() => {
    return this.authService.isStudent();
  });

  public isTeacher = computed(() => {
    return this.authService.isTeacher();
  });

  public goBackToCourse(): void {
    this.router.navigate(['../../'], { relativeTo: this.route });
  }

  public setTab(tab: 'content' | 'grading'): void {
    if (!this.isTeacher) {
      return;
    }
    this.activeTab.set(tab);
  }
  public isAssignmentSubmitMode = signal<boolean>(false);

  ngOnInit(): void {
    this.fetchAssignment();
  }

  public fetchAssignment(): void {
    const id = this.route.snapshot.paramMap.get('assignmentId');
    if (!id) {
      return;
    }
    this.assignmentService
      .fetchAssignment(+id)
      .pipe(
        tap({
          next: (response) => {
            this.assignment.set(response);
          },
        }),
        take(1),
      )
      .subscribe();
  }

  public openAssignmentSubmit(): void {
    if (!this.assignment()) {
      return;
    }
    if (this.isQuiz()) {
      this.router.navigate(['attempt'], {
        relativeTo: this.route,
      });
      return;
    }
    this.isAssignmentSubmitMode.update((val) => !val);
  }

  public cancel(): void {
    this.isAssignmentSubmitMode.set(false);
  }

  public onDeleteAssignment(id: number, title: string) {
    const confirmDelete = confirm(
      `Ви впевнені, що хочете видалити завдання "${title}"?`,
    );

    if (confirmDelete) {
      this.loaderService.busy();

      this.assignmentService
        .deleteAssignment(id)
        .pipe(
          take(1),
          finalize(() => this.loaderService.idle()),
        )
        .subscribe({
          next: () => {
            this.toasterService.success('Завдання видалено');
            this.router.navigate(['../../']);
          },
          error: (err) => {
            this.toasterService.error(
              err.error?.message || 'Помилка при видаленні',
            );
          },
        });
    }
  }
}
