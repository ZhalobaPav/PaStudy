import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { ICourse } from '../../shared/models/course';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private authHelper = inject(HttpAuth);

  public getCourses() {
    return this.authHelper.get<ICourse>('Courses');
  }
}
