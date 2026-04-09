import {
  ChangeDetectorRef,
  Component,
  DestroyRef,
  EventEmitter,
  inject,
  input,
  Input,
  OnInit,
  Output,
  signal,
  TemplateRef,
} from '@angular/core';
import { FetchOptions, Header, TableConfig } from './models/table.models';
import {
  BehaviorSubject,
  debounceTime,
  filter,
  map,
  pairwise,
  Subject,
} from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { safeDetectChanges } from '../../functions/common.functions';
import { isEqual } from 'lodash-es';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
})
export class TableComponent<
  TFilter extends object,
  TRow = unknown,
> implements OnInit {
  ngOnInit(): void {
    this.fetch$
      .pipe(debounceTime(500), takeUntilDestroyed(this.destroyRef))
      .subscribe((options) => {
        const { isNextPage } = options;
        const filters = this.filterState$.value;
        if (isNextPage) {
          this.pageNumber++;
        }
        const fetchOptions = {
          take: this.rowsPerPage,
          skip: this.pageNumber * this.rowsPerPage,
          ...filters,
        };
        this.fetch.emit(fetchOptions);
      });
    this.emitFetch();
    this.filterState$
      .pipe(
        map((state) => (!Object.keys(state || {}).length ? null : state)),
        debounceTime(0),
        pairwise(),
        filter(([prev, curr]) => !isEqual(prev, curr)),
      )
      .subscribe(([prev, currentState]) => {
        this.emitFetch();
      });
    this.gridTemplateColumns.set(this.templateColumn());
  }

  @Input()
  rowTemplate!: TemplateRef<any>;
  @Output()
  fetch = new EventEmitter<FetchOptions<TFilter>>();
  @Input()
  set config(value: TableConfig) {
    this.headers = value.headers;
  }
  public constantColumnNumber = input<number | null>(null);
  @Input()
  set rows(value: any) {
    if (!this.pageNumber) {
      this.rowLines = [...value];
    } else {
      this.rowLines = [...this.rowLines, ...value];
    }
    safeDetectChanges(this.cdr);
  }
  public gridTemplateColumns = signal<string>('');
  public rowLines: any[] = [];
  public headers: Header[] = [];
  public rowsPerPage = 20;
  public pageNumber = 0;

  private destroyRef = inject(DestroyRef);
  private cdr = inject(ChangeDetectorRef);

  private fetch$ = new Subject<{ isNextPage?: boolean }>();
  public filterState$ = new BehaviorSubject<any>(null);
  public resetState$ = new Subject();

  emitFetch(isNextPage?: boolean) {
    this.fetch$.next({ isNextPage: Boolean(isNextPage) });
  }

  templateColumn() {
    return this.headers.map((h) => `${h.width}px`).join(' ');
  }
}
