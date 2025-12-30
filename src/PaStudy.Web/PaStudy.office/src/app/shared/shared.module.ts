import { NgModule } from '@angular/core';
import { TypedTemplate } from './directives/typed-template-directive';
import { UserNamePipe } from './pipes/user-name.pipe';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [],
  imports: [TypedTemplate, UserNamePipe, DatePipe],
  exports: [TypedTemplate, UserNamePipe, DatePipe],
})
export class SharedModule {}
