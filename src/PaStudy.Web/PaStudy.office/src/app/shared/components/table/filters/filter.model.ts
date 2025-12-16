import { UntypedFormGroup } from '@angular/forms';
import { BehaviorSubject, Subject } from 'rxjs';

export interface ITableFilter {
  name: string;
  form: UntypedFormGroup;
  canReset: boolean;
  filterState$: BehaviorSubject<any>;
  resetState$: Subject<any>;
  updateValue(value: any): void;
  setCachedGarages(): void;
  reset(): void;
}
