import { Assignment } from './assignment-item';

export interface Section {
  title: string;
  description: string;
  order?: number;
  courseId: number;
  assignments?: Assignment[];
}
