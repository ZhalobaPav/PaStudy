import { UserRole } from '../../../../shared/enums/userRole';

export interface RegisterModel {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  middleName: string;
  role: UserRole;
  phoneNumber: string;
  displayName: string;
  groupId: number;
  dateOfBirth: Date;
}
