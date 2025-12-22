import { NgModule } from '@angular/core';
import { CoursesComponent } from './courses-list/courses.component';
import { TableModule } from '../../shared/components/table/table.module';
import { CourseRoutingModule } from './course-routing.module';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [CoursesComponent],
  imports: [TableModule, CourseRoutingModule, SharedModule],
})
export class CoursesModule {}
