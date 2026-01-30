import { NgModule } from '@angular/core';
import { CreateAssignmentComponent } from './create-assignment/create-assignment.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AssignmentRoutingModule } from './assignment.routing-module';
import { AssignmentListComponent } from './assignment-list/assignment-list.component';
import { AssignmentItemComponent } from './assignment-item/assignment-item.component';
import { RouterLink } from '@angular/router';
import { PaInputComponent } from '../../../shared/components/pa-input/pa-input.component';
import { SectionItemComponent } from './section-item/section-item.component';

@NgModule({
  declarations: [
    CreateAssignmentComponent,
    AssignmentListComponent,
    AssignmentItemComponent,
    SectionItemComponent,
  ],
  imports: [
    ReactiveFormsModule,
    AssignmentRoutingModule,
    RouterLink,
    PaInputComponent,
  ],
  exports: [AssignmentListComponent],
})
export class AssignmentModule {}
