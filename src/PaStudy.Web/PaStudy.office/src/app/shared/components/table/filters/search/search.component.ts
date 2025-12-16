import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { TableFilterBase } from '../filter-base';
import { UntypedFormBuilder, ɵInternalFormsSharedModule } from '@angular/forms';
import { debounce, filter, first, map, of, take, tap, timer } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ITableFilter } from '../filter.model';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [ɵInternalFormsSharedModule],
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss',
})
export class SearchFilterComponent
  extends TableFilterBase
  implements OnInit, ITableFilter
{
  @Input()
  inputType: 'number' | 'text' = 'text';
  constructor(public fb: UntypedFormBuilder, private destroyRef: DestroyRef) {
    super();
  }
  ngOnInit(): void {
    this.form
      .get('query')
      ?.valueChanges.pipe(
        debounce((value) => {
          return value ? timer(200) : of(null);
        }),
        tap((value) => {
          this.updateValue(value);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }

  form = this.fb.group({
    query: null,
  });

  setCachedGarages() {
    this.filterState$
      .pipe(
        first(),
        filter(Boolean),
        map((state) => state[this.name]),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe((cachedValue: string) => {
        this.form.get('query')?.patchValue(cachedValue, this.emitOptions);
      });
  }
}
