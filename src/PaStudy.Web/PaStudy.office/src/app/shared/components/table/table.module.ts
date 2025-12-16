import { NgModule } from '@angular/core';
import { TableComponent } from './table.component';
import { ScrollViewComponent } from '../scroll-view/scroll-view.component';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [TableComponent],
  imports: [ScrollViewComponent, CommonModule],
  exports: [TableComponent],
})
export class TableModule {}
