import { Component, computed, input, OnInit } from '@angular/core';
import { Assignment } from '../models/assignment-item';
import { AssignmentType } from '../../../../shared/enums/assignment-type';

@Component({
  selector: 'app-assignment-item',
  templateUrl: './assignment-item.component.html',
  styleUrl: './assignment-item.component.scss',
  standalone: false,
})
export class AssignmentItemComponent implements OnInit {
  ngOnInit(): void {
    this.iconSrc = this.defineAssignmnentIcon();
  }
  public assignment = input.required<Assignment | null>();
  public iconSrc = '';

  private iconNames = {
    taskIcon: 'file-svgrepo-com.svg',
    quizIcon: 'exam-svgrepo-com.svg',
  };
  private readonly defaultIconSrc = './../../../../../assets/images/icons/';

  private defineAssignmnentIcon(): string {
    switch (this.assignment()?.assignmentType) {
      case AssignmentType.Task:
        return this.defaultIconSrc + this.iconNames.taskIcon;
      case AssignmentType.Quiz:
        return this.defaultIconSrc + this.iconNames.quizIcon;
      default:
        return this.defaultIconSrc + this.iconNames.taskIcon;
    }
  }

  public isQuiz = computed(
    () => this.assignment()?.assignmentType === AssignmentType.Quiz,
  );
}
