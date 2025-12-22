import { ITeacher } from './teacher';

export interface ICourse {
  id: number;
  title: string;
  categoryName?: string;
  description?: string;
  teachers?: ITeacher[];
}
