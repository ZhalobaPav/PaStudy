import { Pipe, PipeTransform } from '@angular/core';
import {
  SubmissionStatus,
  SUBMISSION_STATUS_LABELS,
} from '../../routes/courses/assignments/models/submission';

@Pipe({ name: 'submissionStatus', standalone: true, pure: true })
export class SubmissionStatusPipe implements PipeTransform {
  transform(value: SubmissionStatus): string {
    return SUBMISSION_STATUS_LABELS[value] ?? '—';
  }
}
