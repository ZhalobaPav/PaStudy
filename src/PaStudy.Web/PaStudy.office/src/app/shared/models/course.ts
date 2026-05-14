import { ITeacher } from './teacher';

export interface ICourse {
  id: number;
  title: string;
  categoryName?: string;
  description?: string;
  teachers?: ITeacher[];
  isEnrolled?: boolean;
  isTeaching?: boolean;
}

export interface CreateCourseDto {
  title: string;
  description: string;
  categoryId: number;
}

export interface CreateCourseResponseDto {
  id: number;
  title: string;
  description: string;
  categoryId: number;
  createdAt: Date;
}
