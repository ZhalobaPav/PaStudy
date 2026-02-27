import { Component, inject, input } from '@angular/core';
import { Section } from '../models/section';
import { CreateAssignmentModalComponent } from '../../../../shared/components/modals/create-assignment-modal/create-assignment-modal.component';
import { ModalService } from '../../../../shared/components/modals/modal.service';

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

  openAssignmentModal() {
    this.modalService
      .open(CreateAssignmentModalComponent, { sectionId: this.section().id })
      .closed.subscribe((res) => {
        console.log(res);
      });
  }
}
