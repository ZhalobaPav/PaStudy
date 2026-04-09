import { Component, inject, input, output, signal } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { PaInputComponent } from '../../../../shared/components/pa-input/pa-input.component';
import { QuestionType } from '../../../../shared/components/table/models/table.models';
import { ScrollViewComponent } from '../../../../shared/components/scroll-view/scroll-view.component';

@Component({
  selector: 'app-quiz-question',
  standalone: true,
  imports: [ReactiveFormsModule, PaInputComponent, ScrollViewComponent],
  templateUrl: './quiz-question.component.html',
  styleUrl: './quiz-question.component.scss',
})
export class QuizQuestionComponent {
  public questionForm = input.required<FormGroup>();
  public index = input.required<number>();
  public remove = output<void>();
  public title = signal<string>('');
  public QuestionType = QuestionType;
  get choiceInfo() {
    return this.questionForm().get('choiceInfo') as FormGroup;
  }
  get matchingInfo() {
    return this.questionForm().get('matchingInfo') as FormGroup;
  }

  get options() {
    return this.choiceInfo?.get('options') as FormArray;
  }
  get pairs() {
    return this.matchingInfo?.get('pairs') as FormArray;
  }

  addOption() {
    this.options.push(this.fb.group({ text: '', isCorrect: false }));
  }
  addPair() {
    this.pairs.push(this.fb.group({ leftSide: '', rightSide: '' }));
  }

  private fb = inject(FormBuilder);
}
