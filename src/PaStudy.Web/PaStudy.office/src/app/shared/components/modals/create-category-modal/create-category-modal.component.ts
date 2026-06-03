import { Component, inject } from '@angular/core';
import { BaseModalComponent } from '../base-modal';
import { HttpClient } from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CategoryService } from './category.service';
import { catchError, finalize, of, take, tap } from 'rxjs';
import { LoaderService } from '../../../services/loader.service';

@Component({
  selector: 'app-create-category-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './create-category-modal.component.html',
  styleUrl: './create-category-modal.component.scss',
})
export class CreateCategoryModalComponent extends BaseModalComponent<{}> {
  private fb = inject(FormBuilder);
  private categoryService = inject(CategoryService);
  private loaderService = inject(LoaderService);
  public form: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
  });

  isSubmitting = false;

  submit() {
    if (this.form.invalid) return;
    this.loaderService.busy();
    this.isSubmitting = true;
    const dto = this.form.value;
    this.categoryService
      .createCategory(dto)
      .pipe(
        tap((dto) => {
          this.isSubmitting = false;
          this.close(dto);
        }),
        catchError((err) => {
          console.error(err);
          return of(null);
        }),
        finalize(() => this.loaderService.idle()),
        take(1),
      )
      .subscribe();
  }
}
