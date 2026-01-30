export enum CourseHeaderTitles {
  Course = 'Курс',
  Students = 'Учасники',
  Assignments = 'Завдання',
  Notes = 'Оцінки',
}

export const headerConfig = [
  {
    title: CourseHeaderTitles.Course,
    path: 'info',
  },
  {
    title: CourseHeaderTitles.Students,
    path: 'students',
  },
  {
    title: CourseHeaderTitles.Notes,
    path: 'notes',
  },
];

export interface HeaderConfig {
  title: CourseHeaderTitles;
  path: string;
}
