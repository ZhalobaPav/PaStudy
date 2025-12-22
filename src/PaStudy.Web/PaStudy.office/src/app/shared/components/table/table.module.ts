import { NgModule } from '@angular/core';
import { TableComponent } from './table.component';
import { ScrollViewComponent } from '../scroll-view/scroll-view.component';
import { CommonModule } from '@angular/common';
import { FilterRerenderComponent } from './filters/filter-rerender/filter-rerender.component';

@NgModule({
  declarations: [TableComponent],
  imports: [ScrollViewComponent, CommonModule, FilterRerenderComponent],
  exports: [TableComponent],
})
export class TableModule {}
