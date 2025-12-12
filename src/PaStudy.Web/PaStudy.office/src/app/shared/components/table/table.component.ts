import {
  Component,
  EventEmitter,
  Input,
  Output,
  TemplateRef,
} from '@angular/core';
import { FetchOptions } from './models/table.models';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
})
export class TableComponent {
  @Input()
  rowTemplate!: TemplateRef<any>;

  @Output()
  fetch = new EventEmitter<FetchOptions>();
  public rowLines: any[] = [];

  set Rows(value: any) {
    this.rowLines = [...this.rowLines, ...value];
  }
}
