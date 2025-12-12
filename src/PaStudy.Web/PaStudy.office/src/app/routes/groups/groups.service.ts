import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { IGroup } from '../../shared/models/group';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GroupsService {
  private httpAuth = inject(HttpAuth);

  public getGroups(): Observable<IGroup[]> {
    return this.httpAuth.get<IGroup[]>('Groups');
  }
}
