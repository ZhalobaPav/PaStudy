import { Component, inject, OnInit, signal } from '@angular/core';
import { BaseModalComponent } from '../base-modal';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { PaInputComponent } from '../../pa-input/pa-input.component';
import { AssignmentService } from '../../../../routes/courses/assignments/assignment.service';
import { Section } from '../../../../routes/courses/assignments/models/section';
import { finalize, take } from 'rxjs';

@Component({
  selector: 'app-create-section-modal',
  standalone: true,
  imports: [PaInputComponent, ReactiveFormsModule],
  templateUrl: './create-section-modal.component.html',
  styleUrl: './create-section-modal.component.scss',
})
export class CreateSectionModalComponent
  extends BaseModalComponent<{ courseId: number }>
  implements OnInit
{
  sectionForm!: FormGroup;
  private fb = inject(FormBuilder);
  private assignmentService = inject(AssignmentService);
  public isLoading = signal<boolean>(false);
  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.sectionForm = this.fb.group({
      title: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(200),
        ],
      ],
      description: ['', [Validators.required]],
      courseId: [this.data.courseId, [Validators.required]],
    });
  }

  public onSubmit(): void {
    if (!this.sectionForm.valid) {
      return;
    }
    this.isLoading.set(true);
    const formValues = this.sectionForm.getRawValue();
    const section: Section = {
      title: formValues.title,
      description: formValues.description,
      courseId: this.data.courseId,
    };

    this.assignmentService
      .createSection(section)
      .pipe(
        take(1),
        finalize(() => {
          this.isLoading.set(false);
        }),
      )
      .subscribe((response) => {
        this.close(response);
      });
  }
}
