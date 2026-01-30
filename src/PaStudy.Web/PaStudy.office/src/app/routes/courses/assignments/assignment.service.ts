import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../../core/services/http-auth';
import { Assignment } from './models/assignment-item';
import { Observable } from 'rxjs';
import { Section } from './models/section';
import { BaseResponse } from '../../../shared/models/base/base-response';

@Injectable({
  providedIn: 'root',
})
export class AssignmentService {
  private httpAuth = inject(HttpAuth);

  fetchSections(courseId: number): Observable<Section[]> {
    return this.httpAuth.get<Section[]>(`assignment/${courseId}`);
  }

  createSection(section: Section): Observable<BaseResponse<Section>> {
    return this.httpAuth.post<BaseResponse<Section>>(
      `assignment/section`,
      section,
    );
  }
  constructor() {}
}
