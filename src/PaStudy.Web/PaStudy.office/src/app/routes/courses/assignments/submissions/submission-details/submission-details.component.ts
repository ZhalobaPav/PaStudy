import {
  Component,
  computed,
  inject,
  input,
  OnInit,
  signal,
} from '@angular/core';
import {
  GradeSubmissionDto,
  TaskSubmission,
  TaskSubmissionDetails,
} from '../../models/assignment-item';
import { AssignmentService } from '../../assignment.service';
import { catchError, finalize, of, switchMap, take, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TextEditorComponent } from '../../../../../shared/components/text-editor/text-editor.component';
import { PendingImage, UploadAttachment } from '../../models/attachment';
import { DatePipe } from '@angular/common';
import { SubmissionStatus } from '../../models/submission';

@Component({
  selector: 'app-submission-details',
  standalone: true,
  imports: [FormsModule, TextEditorComponent, DatePipe],
  templateUrl: './submission-details.component.html',
  styleUrl: './submission-details.component.scss',
})
export class SubmissionDetailsComponent implements OnInit {
  private assignmentService = inject(AssignmentService);
  private route = inject(ActivatedRoute);
  public grade = signal<number | null>(null);
  public feedback = signal<string>('');
  public isLoading = signal<boolean>(false);
  public pendingImages: PendingImage[] = [];
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }
    this.fetchSubmission(+id);
  }
  public submission = signal<TaskSubmissionDetails | null>(null);
  public studentInfo = computed(() => this.submission()?.studentInfo);
  private fetchSubmission(id: number): void {
    this.isLoading.set(true);
    this.assignmentService
      .getSubmissionById(id)
      .pipe(
        tap((response) => {
          this.submission.set(response);
        }),
        finalize(() => this.isLoading.set(false)),
        take(1),
      )
      .subscribe();
  }

  handleImageUpload(pending: PendingImage) {
    this.pendingImages.push(pending);
  }

  public onSave(): void {
    const currentSubmission = this.submission();
    const currentGrade = this.grade();

    if (!currentSubmission || currentGrade === null) {
      alert('Будь ласка, виставте оцінку');
      return;
    }

    this.isLoading.set(true);

    const uploadingFiles$ =
      this.pendingImages.length > 0
        ? this.assignmentService.uploadMultipleFiles(
            this.pendingImages.map((f) => f.file),
          )
        : of([] as UploadAttachment[]);

    uploadingFiles$
      .pipe(
        take(1),
        switchMap((uploadedAttachments: UploadAttachment[]) => {
          let finalFeedback = this.feedback();

          this.pendingImages.forEach((pending, index) => {
            const remoteFile = uploadedAttachments[index];
            if (remoteFile) {
              finalFeedback = finalFeedback
                .split(pending.tempUrl)
                .join(remoteFile.fileUrl);
            }
          });

          const gradeDto: GradeSubmissionDto = {
            submissionId: currentSubmission.id,
            grade: currentGrade,
            teacherFeedback: finalFeedback,
          };

          return this.assignmentService.gradeSubmission(gradeDto);
        }),
        tap((updatedDto) => {
          this.submission.set(updatedDto);

          this.grade.set(updatedDto.grade);
          this.feedback.set(updatedDto.teacherFeedback ?? '');
        }),
        finalize(() => {
          this.isLoading.set(false);
          this.pendingImages.forEach((img) => URL.revokeObjectURL(img.tempUrl));
          this.pendingImages = [];
        }),
        catchError((err) => {
          console.error(err);
          return of(null);
        }),
        take(1),
      )
      .subscribe();
  }
}
