import { NgModule } from '@angular/core';
import { UserListComponent } from './user-list/user-list.component';
import { UserNamePipe } from '../../shared/pipes/user-name.pipe';
import { CommonModule } from '@angular/common';
import { TableModule } from '../../shared/components/table/table.module';
import { UserRoutingModule } from './user-routing-module';

@NgModule({
  declarations: [UserListComponent],
  imports: [UserNamePipe, CommonModule, TableModule, UserRoutingModule],
})
export class UserModule {}
