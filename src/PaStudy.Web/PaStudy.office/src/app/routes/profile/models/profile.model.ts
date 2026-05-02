import { UserRole } from '../../../shared/enums/userRole';
import { ICourse } from '../../../shared/models/course';

export class ProfileInfo {
  fullName!: string;
  email!: string;
  userRole!: UserRole;
  courses: ICourse[] = [];
}
