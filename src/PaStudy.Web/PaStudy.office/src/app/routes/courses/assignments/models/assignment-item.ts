import { Attachment } from './attachment';

export interface Assignment {
  title: string;
  description: string;
  attachments: Attachment[];
  dueDate: Date;
  maxPoints: number;
}
