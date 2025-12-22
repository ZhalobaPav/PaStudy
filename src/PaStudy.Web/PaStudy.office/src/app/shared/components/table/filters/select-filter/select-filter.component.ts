import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { TableFilterBase } from '../filter-base';
import { ITableFilter } from '../filter.model';
import {
  UntypedFormArray,
  UntypedFormBuilder,
  UntypedFormGroup,
  ɵInternalFormsSharedModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { filter, first, map, Observable, of, throttleTime } from 'rxjs';

interface IOption {
  value: string;
  label: string;
}

@Component({
  selector: 'app-select-filter',
  standalone: true,
  imports: [ɵInternalFormsSharedModule, ReactiveFormsModule],
  templateUrl: './select-filter.component.html',
  styleUrl: './select-filter.component.scss',
})
export class SelectFilterComponent
  extends TableFilterBase
  implements ITableFilter, OnInit
{
  @Input()
  options: IOption[] = [];
  @Input()
  optionsRequest!: Observable<IOption[]>;

  label: string = '';
  form: UntypedFormGroup = this.fb.group({
    options: this.fb.array([]),
  });
  optionsFormArray = this.form.get('options') as UntypedFormArray;

  public isCollapsed = true;

  get isAllChecked(): boolean {
    return this.optionsFormArray.value.every((el: any) => el.isChecked);
  }

  constructor(private fb: UntypedFormBuilder, private el: ElementRef) {
    super();
  }

  setCachedValue(): void {
    this.filterState$
      .pipe(
        first(),
        filter(Boolean),
        map((state) => state[this.name])
      )
      .subscribe((cachedValue: string[]) => {
        const formValue = this.optionsFormArray.value;
        cachedValue.forEach((value) => {
          const idx = formValue.findIndex(
            (option: any) => option.value === value
          );
          if (idx >= 0)
            this.optionsFormArray
              .at(idx)
              .get('checked')
              ?.patchValue(true, this.emitOptions);
        });
      });
  }

  private setLabel() {
    const checkedOptions = this.optionsFormArray.value.filter(
      (option: any) => option.isChecked
    );
    const checkedLength = checkedOptions.length;
    const optionsLength = this.optionsFormArray.value.length;

    switch (true) {
      case checkedLength === optionsLength:
        this.label = 'all';
        break;
      case checkedLength === 0:
        this.label = 'none';
        break;
      default:
        this.label = checkedOptions
          .map((option: any) => option.label)
          .join(', ');
        break;
    }
  }

  override reset() {
    this.optionsFormArray.patchValue(
      this.optionsFormArray.value.map((o: any) => ({ ...o, checked: false }))
    );
    this.isCollapsed = true;
  }

  getLabel(form: UntypedFormGroup) {
    return this.form.get('label')?.value;
  }

  setCanReset() {
    const checkedOptions = this.optionsFormArray.value.filter(
      (option: any) => option.checked
    );
    this.canReset = !!checkedOptions.length;
  }

  selectAll() {
    this.optionsFormArray.patchValue(
      this.optionsFormArray.value.map((o: any) => ({ ...o, checked: true }))
    );
    this.isCollapsed = false;
  }

  onClickedOutside(event: Event) {
    if (this.el.nativeElement.contains(event.target)) {
      return;
    }
    this.isCollapsed = true;
  }

  toggleDropdown() {
    this.isCollapsed = !this.isCollapsed;
  }
  ngOnInit(): void {
    const optionsRequest = this.optionsRequest ?? of(this.options);

    optionsRequest.subscribe((options) => {
      options.forEach((option) => {
        const control = this.fb.group({
          label: this.fb.control(option.label),
          value: this.fb.control(option.value),
          checked: this.fb.control(false),
        });
        this.optionsFormArray.push(control);
      });
    });
    this.setLabel();
    this.setCanReset();

    this.optionsFormArray.valueChanges.pipe(throttleTime(100)).subscribe(() => {
      this.setLabel();
      this.setCanReset();
      const value = this.optionsFormArray.value
        .filter((option: any) => option.checked)
        .map((option: any) => option.value);
      this.updateValue(value.length ? value : null);
    });
    this.setCachedValue();
  }
}
