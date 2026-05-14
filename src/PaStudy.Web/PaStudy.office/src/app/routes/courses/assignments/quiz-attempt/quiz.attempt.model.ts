import { QuestionType } from '../../../../shared/components/table/models/table.models';
import { Attachment } from '../models/attachment';

export interface StudentAnswerOption {
  id: number;
  text: string;
}

export interface StudentChoiceInfo {
  options: StudentAnswerOption[];
}

export interface StudentMatchingInfo {
  leftSide: MatchingItemDto[];
  rightSide: MatchingItemDto[];
}

export interface MatchingItemDto {
  id: number;
  text: string;
}

export interface StudentQuestionDto {
  id: number;
  text: string;
  points: number;
  type: QuestionType;
  choiceInfo?: StudentChoiceInfo;
  matchingInfo?: StudentMatchingInfo;
  attachments?: Attachment[];
}

export interface SavedAnswerDto {
  questionId: number;
  selectedOptionId?: number;
  selectedOptionIds?: number[];
  matchingAnswers?: Record<number, number>;
  textResponse?: string;
}

export interface AttemptStartResponseDto {
  attemptId: number;
  quizId: number;
  title: string;
  description?: string;
  timeLimitMinutes: number;
  startedAt: Date;
  dueDate?: Date;
  maxPoints: number;
  questions: StudentQuestionDto[];
  attachments: Attachment[];
  savedAnswers: SavedAnswerDto[];
}
export interface AttemptAnswerPatchDto {
  questionId: number;
  selectedOptionId?: number;
  selectedOptionIds?: number[];
  matchingAnswers?: Record<number, number>;
  textResponse?: string;
}

export interface AttemptResultDto {
  attemptId: number;
  quizId: number;
  totalScore: number;
  maxPoints: number;
  finishedAt: string;
}
