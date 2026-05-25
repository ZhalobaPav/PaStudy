import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../../core/services/http-auth';
import {
  Assignment,
  GradeSubmissionDto,
  TaskSubmission,
  TaskSubmissionDetails,
} from './models/assignment-item';
import { Observable } from 'rxjs';
import { Section } from './models/section';
import { BaseResponse } from '../../../shared/models/base/base-response';
import { UploadAttachment } from './models/attachment';
import {
  CreateSubmissionDto,
  SubmissionFilter,
  SubmissionListItem,
} from './models/submission';
import { HttpParams } from '@angular/common/http';
import {
  AttemptAnswerPatchDto,
  AttemptResultDto,
  AttemptStartResponseDto,
} from './quiz-attempt/quiz.attempt.model';

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

  startQuizAttempt(quizId: number): Observable<any> {
    return this.httpAuth.post<AttemptStartResponseDto>(
      `assignment/quiz/${quizId}/startAttempt`,
      {},
    );
  }

  createQuiz(quizData: any): Observable<BaseResponse<any>> {
    return this.httpAuth.post<BaseResponse<any>>('assignment', quizData);
  }

  submitAssignment(submission: CreateSubmissionDto) {
    return this.httpAuth.post<void>('submission', submission);
  }

  fetchSubmissions(
    filter: SubmissionFilter,
    assignmentId: number,
  ): Observable<SubmissionListItem[]> {
    let params = new HttpParams().set('assignmentId', assignmentId.toString());

    if (filter.pageNumber)
      params = params.set('pageNumber', filter.pageNumber.toString());
    if (filter.pageSize)
      params = params.set('pageSize', filter.pageSize.toString());
    if (filter.status !== undefined)
      params = params.set('status', filter.status.toString());
    return this.httpAuth.get<SubmissionListItem[]>('submission', params);
  }

  getSubmissionById(submissionId: number): Observable<TaskSubmissionDetails> {
    return this.httpAuth.get<TaskSubmissionDetails>(
      `submission/${submissionId}`,
    );
  }

  getQuizById(attemptId: number): Observable<AttemptStartResponseDto> {
    return this.httpAuth.get<AttemptStartResponseDto>(
      `submission/quizAttempt/${attemptId}`,
    );
  }

  public gradeSubmission(
    data: GradeSubmissionDto,
  ): Observable<TaskSubmissionDetails> {
    return this.httpAuth.put<TaskSubmissionDetails>('submission', data);
  }

  public saveAnswer(
    attemptId: number,
    dto: AttemptAnswerPatchDto,
  ): Observable<void> {
    return this.httpAuth.patch<void>(`assignment/attempts/${attemptId}`, dto);
  }

  public submitAnswer(attemptId: number): Observable<AttemptResultDto> {
    return this.httpAuth.post<AttemptResultDto>(
      `assignment/attempts/${attemptId}/submit`,
      {},
    );
  }

  public deleteAssignment(id: number): Observable<any> {
    return this.httpAuth.delete(`assignment/${id}`);
  }

  constructor() {}
}
