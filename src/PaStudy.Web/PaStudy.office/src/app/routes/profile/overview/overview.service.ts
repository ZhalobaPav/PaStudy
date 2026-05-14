import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../../core/services/http-auth';
import { ProfileInfo } from '../models/profile.model';
import { Observable } from 'rxjs';
import { CoursesFilter } from '../../courses/models/courses-filter';
import { CourseQuantity } from '../../../shared/enums/course-quantity-type';
import { Category } from '../../../shared/models/category';

@Injectable({
  providedIn: 'root',
})
export class OverviewService {
  private http = inject(HttpAuth);

  public getProfileInfo(
    filter: CoursesFilter | null = null,
  ): Observable<ProfileInfo> {
    const urlParams = new URLSearchParams();
    urlParams.append('courseQuantity', CourseQuantity.Enrolled.toString());
    return this.http.get<ProfileInfo>(`overview?${urlParams.toString()}`);
  }

  public getInfoForCreateCourse(): Observable<Category[]> {
    return this.http.get<Category[]>('Courses/Categories');
  }
  constructor() {}
}
