import { AssignmentType } from '../../../../shared/enums/assignment-type';
import { Attachment } from './attachment';

export interface Assignment {
  id: number;
  title: string;
  description: string;
  attachments: Attachment[];
  dueDate: Date;
  maxPoints: number;
  assignmentType: AssignmentType;
  quizInfo?: QuizInfoBrieft;
}

export interface QuizInfoBrieft {
  shuffleQuestions: boolean;
  timeLimitMinutes: number;
  questionQuantity: number;
}
