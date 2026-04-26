import {
  AfterViewChecked,
  AfterViewInit,
  Component,
  computed,
  ElementRef,
  input,
  signal,
  ViewChild,
} from '@angular/core';
import { Assignment } from '../models/assignment-item';
import { DatePipe, NgClass } from '@angular/common';
import { AssignmentType } from '../../../../shared/enums/assignment-type';

@Component({
  selector: 'app-assignment-status',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './assignment-status.component.html',
  styleUrl: './assignment-status.component.scss',
})
export class AssignmentStatusComponent implements AfterViewChecked {
  ngAfterViewChecked(): void {
    this.checkOverflow();
  }
  @ViewChild('noteContainer') noteContainer!: ElementRef;
  public assignment = input.required<Assignment>();

  public submissionInfo = computed(() => {
    return this.assignment().assignmentType === AssignmentType.Task
      ? this.assignment().submissionInfo?.taskSubmission
      : null;
  });
  public hasOverflow = signal<boolean>(false);
  public isExpand = signal<boolean>(false);
  public isSubmited = computed(() => {
    return !!this.assignment()?.submissionInfo?.isSubmitted;
  });
  public timeLeft = computed(() => {
    const dueDate = new Date(this.assignment().dueDate).getTime();
    const now = new Date().getTime();
    const diff = dueDate - now;

    const isOverdue = diff < 0;
    const absDiff = Math.abs(diff);

    const days = Math.floor(absDiff / (1000 * 60 * 60 * 24));
    const hours = Math.floor(
      (absDiff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60),
    );
    const minutes = Math.floor((absDiff % (1000 * 60 * 60)) / (1000 * 60));

    let timeString = '';
    if (days > 0) timeString += `${days} дн. `;
    if (hours > 0) timeString += `${hours} год. `;
    timeString += `${minutes} хв.`;

    return {
      text: isOverdue ? `Завдання здано пізніше на ${timeString}` : timeString,
      isOverdue,
    };
  });
  public expand = () => this.isExpand.update((val) => !val);

  private checkOverflow() {
    if (this.noteContainer) {
      const element = this.noteContainer.nativeElement;
      const overflow = element.scrollHeight > element.clientHeight;

      if (this.hasOverflow() !== overflow && !this.isExpand()) {
        this.hasOverflow.set(overflow);
      }
    }
  }
}
