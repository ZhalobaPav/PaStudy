import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { Observable } from 'rxjs';
import { Notification } from '../models/notification';

@Injectable({ providedIn: 'root' })
export class MessagingService {
  private httpAuth = inject(HttpAuth);

  getNotifications(): Observable<Notification[]> {
    return this.httpAuth.get<Notification[]>('notification');
  }
}
