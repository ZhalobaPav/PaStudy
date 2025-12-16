import { Directive, input, Input, OnInit } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Directive()
export abstract class TableFilterBase {
  @Input()
  name!: string;
  @Input()
  placeholder: string = '';

  @Input()
  filterState$!: BehaviorSubject<any>;

  @Input()
  resetState$!: Subject<any>;

  emitOptions = { emitEvent: false };
  canReset = false;
  reset() {}

  updateValue(value: any) {
    const updateState = {
      ...this.filterState$.value,
      [this.name]: value,
    };

    if (!value) {
      delete updateState[this.name];
    }

    this.filterState$.next(updateState);
  }
}
