import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { UserFilter } from './models/user-filter';
import { User } from '../../shared/models/user';
import { FetchOptions } from '../../shared/components/table/models/table.models';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private httpAuth = inject(HttpAuth);

  public fetchUsers(options: FetchOptions<UserFilter>) {
    let params = new HttpParams();
    if (options.searchTerm)
      params = params.set('searchTerm', options.searchTerm);
    if (options.sortOrder) params = params.set('sortOrder', options.sortOrder);
    if (options.courseId) params = params.set('courseId', options.courseId);

    return this.httpAuth.get<User[]>('users', params);
  }
  constructor() {}
}
