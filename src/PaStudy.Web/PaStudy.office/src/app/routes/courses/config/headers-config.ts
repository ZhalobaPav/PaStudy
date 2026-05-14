export enum CourseHeaderTitles {
  Course = 'Курс',
  Students = 'Учасники',
  Assignments = 'Завдання',
  Notes = 'Оцінки',
}

export enum RoleGuardEnum {
  Student = 0,
  Teacher = 1,
  Both = 2,
}

export const headerConfig = [
  {
    title: CourseHeaderTitles.Course,
    path: 'info',
    role: RoleGuardEnum.Both,
  },
  {
    title: CourseHeaderTitles.Students,
    path: 'students',
    role: RoleGuardEnum.Both,
  },
  {
    title: CourseHeaderTitles.Notes,
    path: 'notes',
    role: RoleGuardEnum.Student,
  },
];

export interface HeaderConfig {
  title: CourseHeaderTitles;
  path: string;
  role?: RoleGuardEnum;
}
