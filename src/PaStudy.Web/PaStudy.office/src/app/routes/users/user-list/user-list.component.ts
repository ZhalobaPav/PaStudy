import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { UserFilter } from '../models/user-filter';
import { UserService } from '../user.service';
import { TableComponent } from '../../../shared/components/table/table.component';
import { take, tap } from 'rxjs';
import { FilterUserProfile } from '../enums/filterUserProfile';
import {
  FetchOptions,
  Header,
  TableConfig,
} from '../../../shared/components/table/models/table.models';
import { User } from '../../../shared/models/user';
import { tableConfig } from '../helpers/user-list-table.config';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss',
})
export class UserListComponent implements OnInit {
  ngOnInit(): void {
    this.tableConfig = tableConfig;
  }
  private userService = inject(UserService);
  @ViewChild(TableComponent, { static: true })
  tableComponent!: TableComponent<UserFilter>;
  users: User[] = [];
  public tableConfig!: TableConfig;

  public fetchUsers(options: FetchOptions<UserFilter>) {
    options.filterUserProfile ??= FilterUserProfile.OnlyStudents;
    this.userService
      .fetchUsers(options)
      .pipe(
        take(1),
        tap((response) => {
          this.users = response;
        })
      )
      .subscribe();
  }
}
