import {
  Component,
  effect,
  inject,
  input,
  OnInit,
  output,
  signal,
} from '@angular/core';
import { ICourse } from '../../../shared/models/course';
import { CourseService } from '../course.service';
import { catchError, finalize, take, tap } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryService } from '../../../shared/components/modals/create-category-modal/category.service';
import { Category } from '../../../shared/models/category';
import { of } from 'rxjs';

@Component({
  selector: 'app-course-info',
  templateUrl: './course-info.component.html',
  styleUrl: './course-info.component.scss',
  standalone: false,
})
export class CourseInfoComponent implements OnInit {
  private courseService = inject(CourseService);
  private categoryService = inject(CategoryService);
  public course = input.required<ICourse | null>();
  public isEditMode = input.required<boolean>();
  public courseUpdated = output<void>();
  public categories = signal<Category[]>([]);
  private fb = inject(FormBuilder);
  public infoForm!: FormGroup;
  public isSubmitting = signal<boolean>(false);

  constructor() {
    effect(() => {
      const currentCourse = this.course();
      if (currentCourse) {
        this.infoForm.patchValue({
          title: currentCourse.title,
          description: currentCourse.description,
          categoryId: currentCourse.categoryId,
        });
      }
    });
  }
  ngOnInit(): void {
    this.loadCategories();
    this.infoForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required]],
      categoryId: [null, [Validators.required]],
    });
  }

  public saveInfo(): void {
    const currentCourse = this.course();
    if (this.infoForm.invalid || !currentCourse || this.isSubmitting()) return;

    this.isSubmitting.set(true);
    const { title, description } = this.infoForm.getRawValue();

    this.courseService
      .updateCourse(
        currentCourse.id,
        title,
        description,
        currentCourse.categoryId,
      )
      .pipe(
        tap(() => {
          this.courseUpdated.emit();
        }),

        catchError((err) => {
          console.error('Помилка при збереженні інформації про курс:', err);
          return of(null);
        }),
        finalize(() => {
          this.isSubmitting.set(false);
        }),
        take(1),
      )
      .subscribe();
  }

  private loadCategories(): void {
    this.categoryService
      .getCategories()
      .pipe(
        tap((categories) => {
          this.categories.set(categories);
        }),
        take(1),
      )
      .subscribe();
  }
}
