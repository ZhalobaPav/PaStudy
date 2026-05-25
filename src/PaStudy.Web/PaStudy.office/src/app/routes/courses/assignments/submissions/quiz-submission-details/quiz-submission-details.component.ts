import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AssignmentService } from '../../assignment.service';
import { finalize, take, tap } from 'rxjs';
import {
  AttemptStartResponseDto,
  StudentQuestionDto,
} from '../../quiz-attempt/quiz.attempt.model';
import { LoaderService } from '../../../../../shared/services/loader.service';
import { NgClass } from '@angular/common';
import { QuestionType } from '../../../../../shared/components/table/models/table.models';

@Component({
  selector: 'app-quiz-submission-details',
  standalone: true,
  imports: [NgClass],
  templateUrl: './quiz-submission-details.component.html',
  styleUrl: './quiz-submission-details.component.scss',
})
export class QuizSubmissionDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private quizService = inject(AssignmentService);
  public quizInfo = signal<AttemptStartResponseDto | null>(null);
  private loaderService = inject(LoaderService);
  public readonly type = QuestionType;
  ngOnInit(): void {
    this.fetchQuiSubmission();
  }

  private fetchQuiSubmission() {
    this.loaderService.busy();
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.loaderService.idle();
      console.error('there is no quiz with such id');
      return;
    }
    this.quizService
      .getQuizById(+id)
      .pipe(
        tap((response) => {
          this.quizInfo.set(response);
        }),
        finalize(() => this.loaderService.idle()),
        take(1),
      )
      .subscribe();
  }
  public isOptionSelected(questionId: number, optionId: number): boolean {
    const savedAnswers = this.quizInfo()?.savedAnswers ?? [];
    const answer = savedAnswers.find((a) => a.questionId === questionId);
    if (!answer) return false;

    if (answer.selectedOptionId === optionId) return true;
    if (answer.selectedOptionIds?.includes(optionId)) return true;

    return false;
  }

  public getUserMatchingAnswer(
    questionId: number,
    pairId: number,
  ): number | null {
    const savedAnswers = this.quizInfo()?.savedAnswers ?? [];
    const answer = savedAnswers.find((a) => a.questionId === questionId);
    if (!answer || !answer.matchingAnswers) return null;

    const rightIdStr = answer.matchingAnswers[pairId];
    return rightIdStr ? +rightIdStr : null;
  }

  public getRightSideTextById(
    question: StudentQuestionDto,
    rightSideId: number | null,
  ): string | null {
    if (!rightSideId) return null;
    const rightItem = question.matchingInfo?.rightSide.find(
      (r) => r.id === rightSideId,
    );
    return rightItem?.text ?? null;
  }

  // Правильна права частина для лівого елемента
  // leftItem.id === правильний rightItem.id (бо пари мають однаковий id)
  public getCorrectRightSideText(
    question: StudentQuestionDto,
    leftItemId: number,
  ): string | null {
    const rightItem = question.matchingInfo?.rightSide.find(
      (r) => r.id === leftItemId,
    );
    return rightItem?.text ?? null;
  }

  // Повертає текст відкритої відповіді
  public getTextResponse(questionId: number): string | null {
    const savedAnswers = this.quizInfo()?.savedAnswers ?? [];
    const answer = savedAnswers.find((a) => a.questionId === questionId);
    return answer?.textResponse ?? null;
  }
}
