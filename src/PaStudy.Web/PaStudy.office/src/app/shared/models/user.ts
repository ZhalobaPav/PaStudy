import { UserRole } from '../enums/userRole';

export interface User {
  [prop: string]: any;

  id: string;
  firstName: string;
  lastName: string;
  middleName: string;
  dateOfBirth: Date;
  email: string;
  avatar?: string;
  roles?: any[];
  permissions?: any[];
  userRole: UserRole;
  groupId: number;
}
