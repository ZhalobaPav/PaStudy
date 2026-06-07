import { AssignmentType } from '../../../../shared/enums/assignment-type';
import { Attachment } from './attachment';
import { SubmissionStatus } from './submission';

export interface Assignment {
  id: number;
  title: string;
  description: string;
  attachments: Attachment[];
  dueDate: Date;
  maxPoints: number;
  assignmentType: AssignmentType;
  quizInfo?: QuizInfoBrieft;
  submissionInfo?: SubmissionInfo | null;
  status?: AssignmentStatus;
}

export enum AssignmentStatus {
  NotStarted = 0,
  Submited = 1,
  Graded = 2,
}

export interface QuizInfoBrieft {
  shuffleQuestions: boolean;
  timeLimitMinutes: number;
  questionQuantity: number;
}

export interface SubmissionInfo {
  isSubmitted: boolean;
  submittedAt?: Date | string | null;
  grade?: number | null;
  teacherFeedback?: string | null;
  taskSubmission?: TaskSubmission | null;
  quizSubmission?: QuizSubmissionBrief | null;
}
export interface QuizSubmissionBrief {
  attemptId: number;
  attemptNumber: number;
  grade: number;
  status: string;
}

export interface TaskSubmission {
  studentNote: string;
  attachments: Attachment[];
}

export interface TaskSubmissionDetails {
  id: number;
  studentNotes: string;
  attachments: Attachment[];
  teacherFeedback?: string;
  grade: number | null;
  studentInfo: StudentInfoBrief;
  assignmentInfo: AssignmentInfo;
  status: SubmissionStatus;
  submittedAt?: Date;
}

export interface StudentInfoBrief {
  studentFullName?: string;
  studentEmail?: string;
}

export interface AssignmentInfo {
  title: string;
  description: string;
  dueDate?: Date;
  maxPoints: number;
}

export interface QuizInfoBrief {
  shuffleQuestions: boolean;
  timeLimitMinutes: number;
  questionQuantity: number;
}

export interface GradeSubmissionDto {
  grade: number;
  teacherFeedback: string;
  submissionId: number;
}
