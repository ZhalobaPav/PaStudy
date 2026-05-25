import {
  Component,
  computed,
  DestroyRef,
  HostListener,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { HttpAuth } from '../../../../core/services/http-auth';
import { ActivatedRoute, Router } from '@angular/router';
import { AssignmentService } from '../assignment.service';
import { catchError, finalize, interval, of, take, tap } from 'rxjs';
import {
  AttemptAnswerPatchDto,
  AttemptStartResponseDto,
  SavedAnswerDto,
} from './quiz.attempt.model';
import { QuizCardComponent } from '../quiz-card/quiz-card.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LoaderService } from '../../../../shared/services/loader.service';

@Component({
  selector: 'app-quiz-attempt',
  standalone: true,
  imports: [QuizCardComponent],
  templateUrl: './quiz-attempt.component.html',
  styleUrl: './quiz-attempt.component.scss',
})
export class QuizAttemptComponent implements OnInit {
  private http = inject(HttpAuth);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);
  private router = inject(Router);
  private loaderService = inject(LoaderService);
  private assignmentService = inject(AssignmentService);
  public attemptInfo = signal<AttemptStartResponseDto | null>(null);
  public isLoading = computed<boolean>(() => this.loaderService.isLoading());
  public activeQuestionIndex = signal<number>(0);
  public timeLeftSeconds = signal<number>(0);
  public formattedTime = computed(() => {
    const totalSeconds = this.timeLeftSeconds();
    if (totalSeconds <= 0) return '00:00';

    const m = Math.floor(totalSeconds / 60);
    const s = totalSeconds % 60;
    return `${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}`;
  });
  public tabSwitches = signal<number>(0);
  private maxTabSwitches = 3;
  ngOnInit(): void {
    this.startQuizAttempt();
  }

  public savedAnswersMap = computed(() => {
    const attempt = this.attemptInfo();
    if (!attempt || !attempt.savedAnswers) return {};
    return attempt.savedAnswers.reduce(
      (acc, answer) => {
        acc[answer.questionId] = answer;
        return acc;
      },
      {} as Record<number, SavedAnswerDto>,
    );
  });

  // @HostListener('document:visibilitychange')
  // onVisibilityChange() {
  //   if (document.hidden && this.attemptInfo()) {
  //     const currentSwitches = this.tabSwitches() + 1;
  //     this.tabSwitches.set(currentSwitches);

  //     if (currentSwitches >= this.maxTabSwitches) {
  //       alert(
  //         'Ви занадто багато разів покидали вкладку з тестом! Роботу завершено автоматично.',
  //       );
  //       this.submitQuiz();
  //     } else {
  //       alert(
  //         `Увага! Виходити з вкладки заборонено. Порушення ${currentSwitches}/${this.maxTabSwitches}.`,
  //       );
  //     }
  //   }
  // }

  @HostListener('contextmenu', ['$event'])
  onRightClick(event: MouseEvent) {
    event.preventDefault();
  }

  @HostListener('copy', ['$event'])
  onCopy(event: ClipboardEvent) {
    event.preventDefault();
    alert('Копіювання заборонено!');
  }

  private startQuizAttempt() {
    this.loaderService.busy();
    const id = this.route.snapshot.paramMap.get('assignmentId');
    if (!id) {
      console.error('no id in the root');
      this.loaderService.idle();
      return;
    }
    this.assignmentService
      .startQuizAttempt(+id)
      .pipe(
        tap((response) => {
          this.initTimer(response);
          this.attemptInfo.set(response);
          this.restoreActiveQuestion(response);
        }),
        finalize(() => {
          this.loaderService.idle();
        }),
        take(1),
      )
      .subscribe();
  }

  private restoreActiveQuestion(attempt: AttemptStartResponseDto): void {
    if (!attempt.savedAnswers || attempt.savedAnswers.length === 0) {
      this.activeQuestionIndex.set(0);
      return;
    }

    const firstUnansweredIndex = attempt.questions.findIndex(
      (q) => !attempt.savedAnswers.some((ans) => ans.questionId === q.id),
    );

    if (firstUnansweredIndex !== -1) {
      this.activeQuestionIndex.set(firstUnansweredIndex);
    } else {
      this.activeQuestionIndex.set(attempt.questions.length - 1);
    }
  }

  public onSaveAnswer(patchDto: AttemptAnswerPatchDto): void {
    this.assignmentService
      .saveAnswer(this.attemptInfo()!.attemptId, patchDto)
      .subscribe({
        next: () => {
          console.log(
            `Відповідь на питання ${patchDto.questionId} успішно збережена.`,
          );
        },
        error: (err) => {
          console.error('Не вдалося зберегти відповідь', err);
        },
      });
  }

  public submitQuiz(): void {
    this.loaderService.busy();
    this.assignmentService
      .submitAnswer(this.attemptInfo()!.attemptId)
      .pipe(
        catchError((err) => {
          console.error(err);
          return of(err);
        }),
        finalize(() => {
          this.loaderService.idle();
          this.router.navigate(['../'], { relativeTo: this.route });
        }),
        take(1),
      )
      .subscribe();
  }

  private initTimer(attempt: AttemptStartResponseDto): void {
    const startTime = new Date(attempt.startedAt).getTime();
    const limitMs = attempt.timeLimitMinutes * 60 * 1000;
    const endTime = startTime + limitMs;

    interval(1000)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        const now = new Date().getTime();
        const remaining = Math.max(0, Math.floor((endTime - now) / 1000));

        this.timeLeftSeconds.set(remaining);

        if (remaining === 0) {
          this.submitQuiz();
        }
      });
  }
}
