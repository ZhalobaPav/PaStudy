import { NgModule } from '@angular/core';
import { TableComponent } from './table.component';
import { ScrollViewComponent } from '../scroll-view/scroll-view.component';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [TableComponent],
  imports: [ScrollViewComponent, BrowserModule],
})
export class TableModule {}
