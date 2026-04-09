import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { ICourse } from '../../shared/models/course';
import { CoursesFilter } from './models/courses-filter';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private authHelper = inject(HttpAuth);

  public getCourses(coursesFilter: CoursesFilter) {
    const urlParams = new URLSearchParams();
    if (coursesFilter.searchTerm)
      urlParams.append('searchTerm', coursesFilter.searchTerm);
    if (
      coursesFilter.courseQuantity !== undefined &&
      coursesFilter.courseQuantity !== null
    ) {
      urlParams.append(
        'courseQuantity',
        coursesFilter.courseQuantity.toString(),
      );
    }
    return this.authHelper.get<ICourse[]>(`Courses?${urlParams.toString()}`);
  }

  public getCourse(id: number) {
    return this.authHelper.get<ICourse>(`Courses/${id}`);
  }

  public enrollToCourse(id: number) {
    return this.authHelper.post<void>(`Courses/${id}/enroll`, {});
  }
}
