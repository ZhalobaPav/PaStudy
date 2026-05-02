import { ChangeDetectorRef, ViewRef } from '@angular/core';
import { UserRole } from '../enums/userRole';

export function safeDetectChanges(cdr: ChangeDetectorRef) {
  cdr && !(cdr as ViewRef).destroyed && cdr.detectChanges();
}

export function getRoleName(role: UserRole | undefined) {
  switch (role) {
    case UserRole.Student:
      return 'Студент';
    case UserRole.Teacher:
      return 'Викладач';
    default:
      return 'Користувач';
  }
}
