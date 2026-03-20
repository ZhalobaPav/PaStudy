import { Component, inject, OnInit, signal } from '@angular/core';
import { Assignment } from '../models/assignment-item';
import { ActivatedRoute } from '@angular/router';
import { AssignmentService } from '../assignment.service';
import { take, tap } from 'rxjs';

@Component({
  selector: 'app-assignment-details',
  standalone: false,
  templateUrl: './assignment-details.component.html',
  styleUrl: './assignment-details.component.scss',
})
export class AssignmentDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private assignmentService = inject(AssignmentService);

  ngOnInit(): void {
    this.fetchAssignment();
  }

  private fetchAssignment() {
    const id = this.route.snapshot.paramMap.get('assignmentId');
    if (!id) {
      return;
    }
    this.assignmentService
      .fetchAssignment(+id)
      .pipe(
        tap({
          next: (response) => this.assignment.set(response),
        }),
        take(1),
      )
      .subscribe();
  }

  public assignment = signal<Assignment | null>(null);
}
