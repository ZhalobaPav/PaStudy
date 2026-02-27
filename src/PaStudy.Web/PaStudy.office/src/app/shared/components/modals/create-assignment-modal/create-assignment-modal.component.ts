import { Component, inject, OnInit, signal } from '@angular/core';
import { BaseModalComponent } from '../base-modal';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AssignmentService } from '../../../../routes/courses/assignments/assignment.service';
import { AssignmentType } from '../../../enums/assignment-type';
import { take, tap } from 'rxjs';
import { TextEditorComponent } from '../../text-editor/text-editor.component';
import { ScrollViewComponent } from '../../scroll-view/scroll-view.component';
interface AssignmentForm {
  title: FormControl<string>;
  description: FormControl<string | null>;
  dueDate: FormControl<Date | null>;
  maxPoints: FormControl<number>;
  attachments: FormArray<FormGroup<AttachmentFormGroup>>;
  assignmentType: FormControl<AssignmentType>;
}

interface AttachmentFormGroup {
  fileName: FormControl<string>;
  fileUrl: FormControl<string>;
  contentType: FormControl<string>;
}

@Component({
  selector: 'app-create-assignment-modal',
  standalone: true,
  imports: [ReactiveFormsModule, TextEditorComponent, ScrollViewComponent],
  templateUrl: './create-assignment-modal.component.html',
  styleUrl: './create-assignment-modal.component.scss',
})
export class CreateAssignmentModalComponent
  extends BaseModalComponent<{
    sectionId: number;
  }>
  implements OnInit
{
  ngOnInit(): void {
    this.initForm();
  }
  public assignmentForm!: FormGroup;
  private fb = inject(FormBuilder);
  private assignmentService = inject(AssignmentService);
  public isLoading = signal<boolean>(false);

  initForm() {
    this.assignmentForm = this.fb.nonNullable.group<AssignmentForm>({
      title: this.fb.nonNullable.control('', [Validators.required]),
      description: this.fb.control('', [Validators.required]),
      dueDate: this.fb.control(null),
      maxPoints: this.fb.nonNullable.control(1),
      attachments: this.fb.array<FormGroup<AttachmentFormGroup>>([]),
      assignmentType: this.fb.nonNullable.control(AssignmentType.Task, [
        Validators.required,
      ]),
    });
  }

  public onSubmit(): void {
    const formValues = this.assignmentForm.getRawValue();
    this.assignmentService
      .createAssignment(formValues)
      .pipe(
        take(1),
        tap((response) => this.close(response)),
      )
      .subscribe();
  }
}
