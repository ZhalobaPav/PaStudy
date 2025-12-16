import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { UserFilter } from './models/user-filter';
import { User } from '../../shared/models/user';
import { FetchOptions } from '../../shared/components/table/models/table.models';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private httpAuth = inject(HttpAuth);

  public fetchUsers(options: FetchOptions<UserFilter>) {
    return this.httpAuth.get<User[]>('users');
  }
  constructor() {}
}
