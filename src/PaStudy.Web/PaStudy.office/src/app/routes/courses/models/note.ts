export interface Note {
  percentage: number;
  grade: number;
  teacherFeedback: string;
  assignmentInfo: {
    id: number;
    name: string;
    maxPoints: number;
  };
}
