import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { AssignmentRoutingModule } from './assignment.routing-module';
import { AssignmentListComponent } from './assignment-list/assignment-list.component';
import { AssignmentItemComponent } from './assignment-item/assignment-item.component';
import { RouterLink } from '@angular/router';
import { PaInputComponent } from '../../../shared/components/pa-input/pa-input.component';
import { SectionItemComponent } from './section-item/section-item.component';
import { AssignmentDetailsComponent } from './assignment-details/assignment-details.component';
import { DatePipe } from '@angular/common';
import { AssignmentStatusComponent } from './assignment-status/assignment-status.component';

@NgModule({
  declarations: [
    AssignmentListComponent,
    AssignmentItemComponent,
    SectionItemComponent,
    AssignmentDetailsComponent,
  ],
  imports: [
    ReactiveFormsModule,
    AssignmentRoutingModule,
    RouterLink,
    PaInputComponent,
    DatePipe,
    AssignmentStatusComponent,
  ],
  exports: [AssignmentListComponent],
})
export class AssignmentModule {}
