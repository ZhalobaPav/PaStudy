import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AssignmentDetailsComponent } from './assignment-details/assignment-details.component';
import { SubmissionDetailsComponent } from './submissions/submission-details/submission-details.component';

const routes: Routes = [
  {
    path: ':assignmentId/quizzes/:id/attempt',
    loadComponent: () =>
      import('./quiz-attempt/quiz-attempt.component').then(
        (m) => m.QuizAttemptComponent,
      ),
  },
  {
    path: ':assignmentId/submission/:id',
    loadComponent: () =>
      import('./submissions/submission-details/submission-details.component').then(
        (m) => m.SubmissionDetailsComponent,
      ),
  },
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
