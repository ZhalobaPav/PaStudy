import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateAssignmentComponent } from './create-assignment/create-assignment.component';

const routes: Routes = [
  { path: 'create', component: CreateAssignmentComponent },
];
@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
})
export class AssignmentRoutingModule {}
