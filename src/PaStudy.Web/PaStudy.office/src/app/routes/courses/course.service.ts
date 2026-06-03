import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import {
  CreateCourseDto,
  CreateCourseResponseDto,
  ICourse,
} from '../../shared/models/course';
import { CoursesFilter } from './models/courses-filter';
import { Note } from './models/note';
import { BaseFilter } from '../../shared/models/base/base-filter-model';
import { Observable } from 'rxjs';
import { TeacherGradebookDto } from './models/teacher-gradebook';

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

  public getNotes(coursesFilter: BaseFilter, courseId: number) {
    const urlParams = new URLSearchParams();
    if (coursesFilter.searchTerm)
      urlParams.append('searchTerm', coursesFilter.searchTerm);
    const query = urlParams.toString();
    const url = `Courses/${courseId}/notes${query ? '?' + query : ''}`;

    return this.authHelper.get<Note[]>(url);
  }

  public getCourse(id: number) {
    return this.authHelper.get<ICourse>(`Courses/${id}`);
  }

  public enrollToCourse(id: number) {
    return this.authHelper.post<void>(`Courses/${id}/enroll`, {});
  }

  public createCourse(
    course: CreateCourseDto,
  ): Observable<CreateCourseResponseDto> {
    return this.authHelper.post('Courses', course);
  }

  public fetchGradebook(id: number): Observable<TeacherGradebookDto[]> {
    return this.authHelper.get<TeacherGradebookDto[]>(
      `Courses/${id}/gradebook`,
    );
  }
  updateCourse(
    id: number,
    title: string,
    description: string,
    categoryId: number | undefined,
  ): Observable<ICourse> {
    return this.authHelper.put<ICourse>(`courses`, {
      id,
      title,
      description,
      categoryId,
    });
  }
}
