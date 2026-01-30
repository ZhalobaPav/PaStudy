import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoursesComponent } from './courses-list/courses.component';
import { CourseDetailsComponent } from './course-details/course-details.component';
import { CourseStudentsComponent } from './course-students/course-students.component';
import { CourseInfoComponent } from './course-info/course-info.component';

const routes: Routes = [
  { path: '', redirectTo: 'course-list', pathMatch: 'full' },
  { path: 'course-list', component: CoursesComponent },
  {
    path: 'course-details/:id',
    component: CourseDetailsComponent,
    children: [
      {
        path: 'assignment',
        loadChildren: () =>
          import('./assignments/assignment.module').then(
            (mod) => mod.AssignmentModule,
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CourseRoutingModule {}
