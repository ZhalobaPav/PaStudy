import { Component, inject, input, OnInit, signal } from '@angular/core';
import { Assignment } from '../models/assignment-item';
import { ActivatedRoute } from '@angular/router';
import { ICourse } from '../../../../shared/models/course';
import { AssignmentService } from '../assignment.service';
import { take, tap } from 'rxjs';
import { Section } from '../models/section';
import { ModalService } from '../../../../shared/components/modals/modal.service';
import { CreateSectionModalComponent } from '../../../../shared/components/modals/create-section-modal/create-section-modal.component';

@Component({
  selector: 'app-assignment-list',
  templateUrl: './assignment-list.component.html',
  styleUrl: './assignment-list.component.scss',
})
export class AssignmentListComponent implements OnInit {
  public assignments = signal<Assignment[]>([]);
  public sections = signal<Section[]>([]);
  private modalService = inject(ModalService);
  private assignmentService = inject(AssignmentService);
  public course = input.required<ICourse | null>();
  public isEditMode = input.required<boolean>();
  ngOnInit(): void {
    this.fetchSections(this.course()?.id);
  }

  fetchSections(courseId: number | undefined) {
    if (!courseId) {
      console.error('course is not find');
      return;
    }
    this.assignmentService
      .fetchSections(courseId)
      .pipe(
        take(1),
        tap((sections) => {
          this.sections.set(sections);
        }),
      )
      .subscribe();
  }

  openModal() {
    this.modalService
      .open(CreateSectionModalComponent, { courseId: this.course()?.id })
      .closed.subscribe((newSection) => {
        if (!newSection) {
          return;
        }
        this.sections.update((list) => [...list, newSection]);
      });
  }
}
