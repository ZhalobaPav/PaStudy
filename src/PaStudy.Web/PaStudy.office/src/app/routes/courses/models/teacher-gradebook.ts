export interface TeacherGradebookDto {
  enrollmentId: number;
  studentId: number;
  studentFullName: string;
  groupName: string;
  finalGrade: number | null;
  progress: number;
  status: string;
  enrolledAt: Date;
}
