export interface Note {
  percentage: number;
  grade: number;
  teacherFeedback: string;
  assignmentInfo: {
    name: string;
    maxPoints: number;
  };
}
