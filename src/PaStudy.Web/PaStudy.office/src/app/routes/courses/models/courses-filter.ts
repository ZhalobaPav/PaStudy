import { CourseQuantity } from '../../../shared/enums/course-quantity-type';

export interface CoursesFilter {
  searchTerm?: string;
  courseId?: number;
  courseQuantity: CourseQuantity;
}
