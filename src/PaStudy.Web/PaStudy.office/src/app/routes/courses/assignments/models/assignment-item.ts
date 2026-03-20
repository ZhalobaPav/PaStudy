import { Attachment } from './attachment';

export interface Assignment {
  id: number;
  title: string;
  description: string;
  attachments: Attachment[];
  dueDate: Date;
  maxPoints: number;
}
