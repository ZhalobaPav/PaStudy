import { ChangeDetectorRef, ViewRef } from '@angular/core';

export function safeDetectChanges(cdr: ChangeDetectorRef) {
  cdr && !(cdr as ViewRef).destroyed && cdr.detectChanges();
}
