import { Component, inject, input, output } from '@angular/core';
import { Section } from '../models/section';
import { CreateAssignmentModalComponent } from '../../../../shared/components/modals/create-assignment-modal/create-assignment-modal.component';
import { ModalService } from '../../../../shared/components/modals/modal.service';
import { CreateQuizBuilderComponent } from '../../../../shared/components/modals/create-quiz-builder/create-quiz-builder.component';

@Component({
  selector: 'app-section-item',
  templateUrl: './section-item.component.html',
  styleUrl: './section-item.component.scss',
  standalone: false,
})
export class SectionItemComponent {
  section = input.required<Section>();
  isEditMode = input.required<boolean>();
  modalService = inject(ModalService);
  assignmentCreated = output<{
    assignment: any;
    sectionId: number | undefined;
  }>();
  openAssignmentModal() {
    this.modalService
      .open(CreateAssignmentModalComponent, { sectionId: this.section().id })
      .closed.subscribe((res) => {
        if (res) {
          this.assignmentCreated.emit({
            assignment: res.data,
            sectionId: this.section().id,
          });
        }
      });
  }

  openQuizModal() {
    this.modalService
      .open(CreateQuizBuilderComponent, { sectionId: this.section().id })
      .closed.subscribe((res) => {
        if (res) {
          this.assignmentCreated.emit({
            assignment: res,
            sectionId: this.section().id,
          });
        }
      });
  }
}
