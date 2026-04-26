import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Assignment } from '../models/assignment-item';
import { ActivatedRoute } from '@angular/router';
import { AssignmentService } from '../assignment.service';
import { take, tap } from 'rxjs';
import { AssignmentType } from '../../../../shared/enums/assignment-type';
import { AuthService } from '../../../auth/auth.service';
import { AssignmentTabType } from '../models/assignment-tab-type';

@Component({
  selector: 'app-assignment-details',
  standalone: false,
  templateUrl: './assignment-details.component.html',
  styleUrl: './assignment-details.component.scss',
})
export class AssignmentDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private assignmentService = inject(AssignmentService);
  private authService = inject(AuthService);

  public assignment = signal<Assignment | null>(null);
  public readonly currentDate = signal<Date>(new Date());
  private readonly typesWithoutNote = [AssignmentType.Reading];
  public readonly AssignmentType = AssignmentType;
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
    this.isAssignmentSubmitMode.update((val) => !val);
  }

  public cancel(): void {
    this.isAssignmentSubmitMode.set(false);
  }
}
