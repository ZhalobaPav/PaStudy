import { Component, computed, inject, OnInit } from '@angular/core';
import { BaseModalComponent } from '../base-modal';
import {
  FormGroup,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  FormControl,
} from '@angular/forms';
import { LoaderService } from '../../../services/loader.service';
import { Category } from '../../../models/category';
import { CourseService } from '../../../../routes/courses/course.service';
import { take, finalize, catchError, of, tap } from 'rxjs';
interface CourseForm {
  title: FormControl<string>;
  description: FormControl<string>;
  categoryId: FormControl<number>;
}
interface CourseInfo {
  categories: Category[];
}
@Component({
  selector: 'app-create-course-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './create-course-modal.component.html',
  styleUrl: './create-course-modal.component.scss',
})
export class CreateCourseModalComponent
  extends BaseModalComponent<{ courseInfo: CourseInfo }>
  implements OnInit
{
  courseForm!: FormGroup;
  private fb = inject(FormBuilder);
  public loaderService = inject(LoaderService);
  private courseService = inject(CourseService);
  public isLoading = computed(() => this.loaderService.isLoading());
  public categories = computed(() => this.data.courseInfo.categories);
  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.courseForm = this.fb.nonNullable.group<CourseForm>({
      title: this.fb.nonNullable.control('', [Validators.required]),
      description: this.fb.nonNullable.control('', [Validators.required]),
      categoryId: this.fb.nonNullable.control(-1, [Validators.required]),
    });
  }

  public onSubmit(): void {
    if (this.courseForm.invalid) {
      this.courseForm.markAllAsTouched();
      return;
    }

    this.loaderService.busy();
    const formValue = this.courseForm.getRawValue();

    this.courseService
      .createCourse(formValue)
      .pipe(
        tap((createdCourse) => {
          this.close(createdCourse);
        }),
        catchError((err) => {
          console.error(err);
          this.close(null);
          return of(null);
        }),
        finalize(() => this.loaderService.idle()),
        take(1),
      )
      .subscribe();
  }
}
