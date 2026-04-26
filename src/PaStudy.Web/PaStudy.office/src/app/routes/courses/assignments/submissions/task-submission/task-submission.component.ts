import {
  Component,
  inject,
  input,
  OnInit,
  output,
  signal,
} from '@angular/core';
import { TextEditorComponent } from '../../../../../shared/components/text-editor/text-editor.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CreateSubmissionDto } from '../../models/submission';
import { PendingImage, UploadAttachment } from '../../models/attachment';
import { AssignmentService } from '../../assignment.service';
import { catchError, finalize, of, switchMap, take, tap } from 'rxjs';
import { AssignmentType } from '../../../../../shared/enums/assignment-type';

@Component({
  selector: 'app-task-submission',
  standalone: true,
  imports: [TextEditorComponent, ReactiveFormsModule],
  templateUrl: './task-submission.component.html',
  styleUrl: './task-submission.component.scss',
})
export class TaskSubmissionComponent implements OnInit {
  ngOnInit(): void {
    this.initForm();
  }
  submissionForm!: FormGroup;
  private fb: FormBuilder = inject(FormBuilder);
  public isLoading = signal<boolean>(false);
  public assignmentId = input.required<number>();
  private assignmentService: AssignmentService = inject(AssignmentService);
  onImageUploaded(pending: PendingImage) {
    this.pendingImages.push(pending);
  }
  public onCancelSubmission = output<void>();
  public onSubmited = output<void>();
  private pendingImages: PendingImage[] = [];
  private initForm() {
    this.submissionForm = this.fb.group({
      content: ['', [Validators.required]],
    });
  }

  onSubmit = () => {
    if (this.submissionForm.invalid) return;
    const formValues = this.submissionForm.getRawValue();
    const uploadingFiles$ =
      this.pendingImages.length > 0
        ? this.assignmentService.uploadMultipleFiles(
            this.pendingImages.map((f) => f.file),
          )
        : of([]);
    uploadingFiles$
      .pipe(
        take(1),
        switchMap((uploadedAttachments: UploadAttachment[]) => {
          let finalNotes = this.submissionForm.getRawValue().content;

          this.pendingImages.forEach((pending, index) => {
            const remoteFile = uploadedAttachments[index];
            if (remoteFile) {
              finalNotes = finalNotes
                .split(pending.tempUrl)
                .join(remoteFile.fileUrl);
            }
          });

          const submissionDto: CreateSubmissionDto = {
            assignmentId: this.assignmentId(),
            assignmentType: AssignmentType.Task,
            taskSubmission: {
              studentNotes: finalNotes,
              attachments: uploadedAttachments,
            },
          };

          return this.assignmentService.submitAssignment(submissionDto);
        }),
        finalize(() => this.isLoading.set(false)),
        tap(() => {
          this.submissionForm.reset();
          this.pendingImages.forEach((img) => URL.revokeObjectURL(img.tempUrl));
          this.pendingImages = [];
          this.onSubmited.emit();
        }),
        catchError((err) => {
          console.error(err);
          return of(null);
        }),
      )
      .subscribe();
  };

  cancel() {
    this.submissionForm.reset();
    this.pendingImages = [];
    this.onCancelSubmission.emit();
  }
}
