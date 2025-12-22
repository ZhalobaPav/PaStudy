import {
  Component,
  Input,
  OnInit,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { Header } from '../../models/table.models';
import { BehaviorSubject, Subject } from 'rxjs';
import { ITableFilter, KeyValueObject } from '../filter.model';
@Component({
  selector: 'pa-filter-rerender',
  templateUrl: './filter-rerender.component.html',
  standalone: true,
})
export class FilterRerenderComponent implements OnInit {
  @Input()
  header!: Header;

  @Input()
  filterState$!: BehaviorSubject<KeyValueObject>;

  @Input()
  resetState$!: Subject<any>;

  @ViewChild('container', { read: ViewContainerRef, static: true })
  container!: ViewContainerRef;

  ngOnInit(): void {
    const { component, componentInputs, name } = this.header;
    if (!component) return;

    const componentRef = this.container.createComponent(component);
    const instance = componentRef.instance as ITableFilter;

    for (const inputName in componentInputs) {
      componentRef.setInput(inputName, componentInputs[inputName]);
    }

    instance.filterState$ = this.filterState$;
    instance.resetState$ = this.resetState$;
    instance.name = name;
  }
}
