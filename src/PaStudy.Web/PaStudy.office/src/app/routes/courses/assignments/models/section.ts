import { Assignment } from './assignment-item';

export interface Section {
  id?: number;
  title: string;
  description: string;
  order?: number;
  courseId: number;
  assignments?: Assignment[];
}
