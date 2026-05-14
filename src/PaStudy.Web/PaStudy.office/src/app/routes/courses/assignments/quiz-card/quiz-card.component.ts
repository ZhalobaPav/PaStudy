import { Component, effect, input, output, signal } from '@angular/core';
import { QuestionType } from '../../../../shared/components/table/models/table.models';
import {
  StudentQuestionDto,
  SavedAnswerDto,
  AttemptAnswerPatchDto,
} from '../quiz-attempt/quiz.attempt.model';

@Component({
  selector: 'app-quiz-card',
  standalone: true,
  imports: [],
  templateUrl: './quiz-card.component.html',
  styleUrl: './quiz-card.component.scss',
})
export class QuizCardComponent {
  public question = input.required<StudentQuestionDto>();
  public index = input.required<number>();
  public savedAnswer = input<SavedAnswerDto | undefined>();
  public selectedOptionIds = signal<number[]>([]);
  public answerChanged = output<AttemptAnswerPatchDto>();

  public selectedOptionId = signal<number | null>(null);
  public matchingSelections = signal<Record<number, number>>({});
  public QuestionType = QuestionType;

  constructor() {
    effect(
      () => {
        const answer = this.savedAnswer();
        if (answer?.selectedOptionId) {
          this.selectedOptionId.set(answer.selectedOptionId);
        }
        if (answer?.matchingAnswers) {
          this.matchingSelections.set(answer.matchingAnswers);
        }
        if (answer?.selectedOptionIds) {
          this.selectedOptionIds.set(answer.selectedOptionIds);
        }
      },
      { allowSignalWrites: true },
    );
  }

  public onOptionSelected(optionId: number): void {
    this.selectedOptionId.set(optionId);

    this.answerChanged.emit({
      questionId: this.question().id,
      selectedOptionId: optionId,
    });
  }

  public onMatchingSelected(leftId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;

    const rightIdStr = selectElement.value;
    const leftIdStr = leftId.toString();

    this.matchingSelections.update((current) => ({
      ...current,
      [leftIdStr]: rightIdStr,
    }));

    this.answerChanged.emit({
      questionId: this.question().id,
      matchingAnswers: this.matchingSelections(),
    });
  }

  public onMultipleOptionToggle(optionId: number, event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const isChecked = checkbox.checked;

    this.selectedOptionIds.update((currentIds) => {
      if (isChecked) {
        return [...currentIds, optionId];
      } else {
        return currentIds.filter((id) => id !== optionId);
      }
    });

    this.answerChanged.emit({
      questionId: this.question().id,
      selectedOptionIds: this.selectedOptionIds(),
    });
  }
}
