export interface Notification {
  id: string;
  title: string;
  message: string;
  type: NotificationType;
  clickActionUrl?: string | null;
  recipientUserId: string;
  courseId?: string | null;
  isRead: boolean;
}

export enum NotificationType {
  CourseInvitation = 1,
  NewAssignment = 2,
  GradeReceived = 3,
  GeneralAnnouncement = 4,
  SubmissionUploaded = 5,
}
