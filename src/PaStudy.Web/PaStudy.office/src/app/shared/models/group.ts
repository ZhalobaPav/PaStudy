import { ITeacher } from './teacher';

export interface IGroup {
  id: number;
  groupNumber: string;
  institutionNumber: string;
  year: number;
  faculty: string;
  speciality: string;
  teacher: ITeacher;
}
