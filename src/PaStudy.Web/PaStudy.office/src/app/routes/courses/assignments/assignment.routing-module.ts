import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AssignmentDetailsComponent } from './assignment-details/assignment-details.component';
import { SubmissionDetailsComponent } from './submissions/submission-details/submission-details.component';

const routes: Routes = [
  {
    path: ':assignmentId',
    component: AssignmentDetailsComponent,
  },
  {
    path: ':assignmentId/submission/:id',
    loadComponent: () =>
      import('./submissions/submission-details/submission-details.component').then(
        (m) => m.SubmissionDetailsComponent,
      ),
  },
];
@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
})
export class AssignmentRoutingModule {}
