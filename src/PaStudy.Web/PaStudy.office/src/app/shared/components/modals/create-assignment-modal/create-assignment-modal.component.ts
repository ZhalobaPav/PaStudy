import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
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
import { finalize, of, switchMap, take, tap } from 'rxjs';
import { TextEditorComponent } from '../../text-editor/text-editor.component';
import { ScrollViewComponent } from '../../scroll-view/scroll-view.component';
import {
  PendingImage,
  UploadAttachment,
} from '../../../../routes/courses/assignments/models/attachment';
interface AssignmentForm {
  title: FormControl<string>;
  description: FormControl<string | null>;
  dueDate: FormControl<Date | null>;
  maxPoints: FormControl<number>;
  attachments: FormArray<FormGroup<AttachmentFormGroup>>;
  assignmentType: FormControl<AssignmentType>;
}

interface ImageInfoFormGroup {
  width: FormControl<number>;
  height: FormControl<number>;
}

interface AttachmentFormGroup {
  fileName: FormControl<string>;
  fileUrl: FormControl<string>;
  contentType: FormControl<string>;
  imageInfo: FormGroup<ImageInfoFormGroup> | FormControl<null>;
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
  implements OnInit, OnDestroy
{
  ngOnDestroy(): void {
    this.pendingImages.forEach((img) => URL.revokeObjectURL(img.tempUrl));
  }
  ngOnInit(): void {
    this.initForm();
  }
  public assignmentForm!: FormGroup;
  private fb = inject(FormBuilder);
  private assignmentService = inject(AssignmentService);
  public isLoading = signal<boolean>(false);
  private pendingImages: PendingImage[] = [];
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
    if (this.assignmentForm.invalid) return;
    this.isLoading.set(true);

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
          const formValues = this.assignmentForm.getRawValue();
          let finalDescription = formValues.description || '';
          this.pendingImages.forEach((pending, index) => {
            const remoteFile = uploadedAttachments[index];
            if (remoteFile) {
              const imgHtml = `<img src="${remoteFile.fileUrl}" alt="${remoteFile.fileName}">`;
              finalDescription = finalDescription.replace(
                `[[${pending.tempUrl}]]`,
                imgHtml,
              );
            }
          });
          const createAssignmentDto = {
            ...formValues,
            description: finalDescription,
            attachments: uploadedAttachments,
            sectionId: this.data.sectionId,
          };
          return this.assignmentService.createAssignment(createAssignmentDto);
        }),
        finalize(() => {
          this.isLoading.set(false);
        }),
      )
      .subscribe({
        next: (response) => {
          this.close(response);
        },
        error: (err) => {
          console.error('Error', err);
        },
      });
  }

  onImageUploaded(pending: PendingImage) {
    this.pendingImages.push(pending);
  }
}
