import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AssignmentDetailsComponent } from './assignment-details/assignment-details.component';

const routes: Routes = [
  {
    path: ':assignmentId',
    component: AssignmentDetailsComponent,
  },
];
@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
})
export class AssignmentRoutingModule {}
