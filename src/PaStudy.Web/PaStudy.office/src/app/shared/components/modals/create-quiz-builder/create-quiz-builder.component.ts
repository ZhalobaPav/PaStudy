import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { PaInputComponent } from '../../pa-input/pa-input.component';
import { BaseModalComponent } from '../base-modal';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';

import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ModalService } from '../modal.service';
import { QuestionType } from '../../table/models/table.models';
import { QuizQuestionComponent } from '../../../../routes/courses/assignments/quiz-question/quiz-question.component';
import { DEFAULT_TIME_LIMIT } from '../../../contsants/base.constants';
import { finalize } from 'rxjs';
import { AssignmentService } from '../../../../routes/courses/assignments/assignment.service';
import { AssignmentType } from '../../../enums/assignment-type';
import { futureDateValidator } from '../../../../routes/auth/shared/validators/laterDate.validator';
interface QuizFormInterface {
  title: FormControl<string>;
  points: FormControl<number>;
  questions: FormArray<FormGroup>;
  timeLimitMinutes: FormControl<number>;
  dueDate: FormControl<Date>;
}
@Component({
  selector: 'app-create-quiz-builder',
  standalone: true,
  imports: [
    PaInputComponent,
    FormsModule,
    ReactiveFormsModule,
    MatMenuModule,
    MatIconModule,
    QuizQuestionComponent,
  ],
  templateUrl: './create-quiz-builder.component.html',
  styleUrl: './create-quiz-builder.component.scss',
})
export class CreateQuizBuilderComponent
  extends BaseModalComponent<{ sectionId: number }>
  implements OnInit
{
  ngOnInit(): void {
    this.initForm();
    this.setupPointsCalculation();
  }
  public quizForm!: FormGroup;
  private fb = inject(FormBuilder);
  private assignmentService = inject(AssignmentService);
  private modalService = inject(ModalService);
  public isLoading = signal<boolean>(false);
  public buttonLabel = computed(() => {
    return this.selectedType() === QuestionType.SingleChoice
      ? `Створити ${this.buttonLabelNames.singleChoice}`
      : `Створити ${this.buttonLabelNames.matchingChoice}`;
  });
  public readonly buttonLabelNames = {
    singleChoice: 'звичайне питання',
    matchingChoice: 'питання-відповідність',
  };

  public readonly QuestionType = QuestionType;
  private selectedType = signal<QuestionType>(QuestionType.SingleChoice);

  private initForm() {
    this.quizForm = this.fb.nonNullable.group<QuizFormInterface>({
      title: this.fb.nonNullable.control('', [Validators.required]),
      points: this.fb.nonNullable.control(0, [Validators.required]),
      questions: this.fb.array<FormGroup>(
        [],
        [Validators.required, Validators.minLength(1)],
      ),
      timeLimitMinutes: this.fb.nonNullable.control(DEFAULT_TIME_LIMIT, [
        Validators.required,
      ]),
      dueDate: this.fb.nonNullable.control(new Date(), [
        Validators.required,
        futureDateValidator(),
      ]),
    });
  }
  get questions() {
    return this.quizForm.get('questions') as FormArray;
  }
  addQuestion() {
    const type = this.selectedType();

    const questionGroup = this.fb.group({
      text: ['', Validators.required],
      points: [1, [Validators.required, Validators.min(1)]],
      type: [type],
      feedback: [''],
      choiceInfo:
        type === QuestionType.SingleChoice
          ? this.fb.group({
              options: this.fb.array([
                this.createOption(),
                this.createOption(),
              ]),
            })
          : null,
      matchingInfo:
        type === QuestionType.Matching
          ? this.fb.group({
              pairs: this.fb.array([this.createPair()]),
            })
          : null,
    });

    this.questions.push(questionGroup);
  }

  private createOption() {
    return this.fb.group({
      text: ['', Validators.required],
      isCorrect: [false],
    });
  }

  private createPair() {
    return this.fb.group({
      leftSide: ['', Validators.required],
      rightSide: ['', Validators.required],
    });
  }

  removeQuestion(index: number) {
    this.questions.removeAt(index);
  }

  private setupPointsCalculation() {
    this.quizForm.get('questions')?.valueChanges.subscribe(() => {
      const totalPoints = this.questions.controls.reduce((sum, control) => {
        const questionPoints = control.get('points')?.value || 0;
        return sum + questionPoints;
      }, 0);

      this.quizForm.patchValue({ points: totalPoints }, { emitEvent: false });
    });
  }

  asFormGroup(control: any): FormGroup {
    return control as FormGroup;
  }

  onSubmit() {
    if (this.quizForm.invalid) {
      this.quizForm.markAllAsTouched();
      return;
    }

    const formValue = this.quizForm.getRawValue();

    const createQuizDto = {
      title: formValue.title,
      description: '',
      maxPoints: formValue.points,
      sectionId: this.data.sectionId,
      assignmentType: AssignmentType.Quiz,
      quizInfo: {
        questions: formValue.questions.map((q: any) => ({
          text: q.text,
          points: q.points,
          type: q.type,
          feedback: q.feedback || '',
          choiceInfo:
            q.type === QuestionType.SingleChoice ? q.choiceInfo : null,
          matchingInfo:
            q.type === QuestionType.Matching ? q.matchingInfo : null,
          attachments: [],
        })),
        timeLimitMinutes: formValue.timeLimitMinutes,
        shuffleQuestions: true,
      },

      attachments: [],
      startDate: new Date(),
      dueDate: formValue.dueDate,
    };

    this.isLoading.set(true);

    this.assignmentService
      .createQuiz(createQuizDto)
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (response) => {
          this.close(response.data);
        },
        error: (err) => {
          console.error('Помилка створення тесту:', err);
        },
      });
  }

  selectType(questionType: QuestionType) {
    this.selectedType.set(questionType);
  }
}
