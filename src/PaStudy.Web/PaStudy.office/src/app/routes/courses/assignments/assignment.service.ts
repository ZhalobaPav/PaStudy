import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../../core/services/http-auth';
import { Assignment } from './models/assignment-item';
import { Observable } from 'rxjs';
import { Section } from './models/section';
import { BaseResponse } from '../../../shared/models/base/base-response';
import { UploadAttachment } from './models/attachment';

@Injectable({
  providedIn: 'root',
})
export class AssignmentService {
  private httpAuth = inject(HttpAuth);

  fetchSections(courseId: number): Observable<Section[]> {
    return this.httpAuth.get<Section[]>(`assignment/sections/${courseId}`);
  }

  fetchAssignment(assignmentId: number): Observable<Assignment> {
    return this.httpAuth.get<Assignment>(`assignment/${assignmentId}`);
  }

  createSection(section: Section): Observable<BaseResponse<Section>> {
    return this.httpAuth.post<BaseResponse<Section>>(
      `assignment/section`,
      section,
    );
  }

  createAssignment(
    assignment: Assignment,
  ): Observable<BaseResponse<Assignment>> {
    return this.httpAuth.post<BaseResponse<Assignment>>(
      'assignment',
      assignment,
    );
  }

  uploadFile(file: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpAuth.post<string>('Files/upload', formData);
  }

  uploadMultipleFiles(files: File[]) {
    const formData = new FormData();
    files.forEach((file) => {
      formData.append('files', file);
    });

    return this.httpAuth.post<UploadAttachment[]>(
      'Files/upload-multiple',
      formData,
    );
  }

  constructor() {}
}
