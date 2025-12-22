import { UntypedFormGroup } from '@angular/forms';
import { BehaviorSubject, Subject } from 'rxjs';

export interface ITableFilter {
  name: string;
  form: UntypedFormGroup;
  canReset: boolean;
  filterState$: BehaviorSubject<any>;
  resetState$: Subject<any>;
  updateValue(value: any): void;
  setCachedValue(): void;
  reset(): void;
}

export type KeyValueObject = Record<string, any>;
