import { Component, inject, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

interface AssignmentForm {
  title: FormControl<string>;
  description: FormControl<string | null>;
  dueDate: FormControl<Date | null>;
  maxPoints: FormControl<number>;
  attachments: FormArray<FormGroup<AttachmentFormGroup>>;
}

interface AttachmentFormGroup {
  fileName: FormControl<string>;
  fileUrl: FormControl<string>;
  contentType: FormControl<string>;
}

@Component({
  selector: 'app-create-assignment',
  templateUrl: './create-assignment.component.html',
  styleUrl: './create-assignment.component.scss',
})
export class CreateAssignmentComponent implements OnInit {
  private formBuilder = inject(FormBuilder);
  public form!: FormGroup;
  public ngOnInit() {
    this.form = this.formBuilder.nonNullable.group<AssignmentForm>({
      title: this.formBuilder.nonNullable.control('', [Validators.required]),
      description: this.formBuilder.control('', [Validators.required]),
      dueDate: this.formBuilder.control(null),
      maxPoints: this.formBuilder.nonNullable.control(100),
      attachments: this.formBuilder.array<FormGroup<AttachmentFormGroup>>([]),
    });
  }
  get attachments() {
    return this.form.controls['attachments'];
  }
}
