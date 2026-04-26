import { NgModule } from '@angular/core';
import { CoursesComponent } from './courses-list/courses.component';
import { TableModule } from '../../shared/components/table/table.module';
import { CourseRoutingModule } from './course-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';
import { CourseDetailsComponent } from './course-details/course-details.component';
import { CourseInfoComponent } from './course-info/course-info.component';
import { CourseStudentsComponent } from './course-students/course-students.component';
import { FormsModule, ɵInternalFormsSharedModule } from '@angular/forms';
import { AssignmentModule } from './assignments/assignment.module';
import { CourseNotesComponent } from './course-notes/course-notes.component';

@NgModule({
  declarations: [
    CoursesComponent,
    CourseDetailsComponent,
    CourseInfoComponent,
    CourseStudentsComponent,
  ],
  imports: [
    TableModule,
    CourseRoutingModule,
    SharedModule,
    RouterModule,
    FormsModule,
    AssignmentModule,
    CourseNotesComponent,
  ],
})
export class CoursesModule {}
