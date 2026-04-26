import { AssignmentType } from '../../../../shared/enums/assignment-type';
import { Attachment } from './attachment';

export interface CreateSubmissionDto {
  assignmentId: number;
  assignmentType: AssignmentType;
  taskSubmission?: CreateTaskSubmission;
  //quizSubmission?: CreateQuizSubmission;
}

export interface CreateTaskSubmission {
  studentNotes: string;
  attachments: Attachment[];
}
export enum SubmissionStatus {
  Draft = 0,
  Submitted = 1,
  Graded = 2,
  LateAndRejected = 3,
  LateAndAccepted = 4,
  Rejected = 5,
}

export interface SubmissionListItem {
  id: number;
  studentId: number;
  studentFullName: string;
  studentEmail: string;
  submittedAt: Date;
  grade?: number;
  status: SubmissionStatus;
}

export interface SubmissionFilter {
  pageNumber?: number;
  pageSize?: number;
  status?: SubmissionStatus;
}
