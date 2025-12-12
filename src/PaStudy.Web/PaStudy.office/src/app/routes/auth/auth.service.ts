import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { Router } from '@angular/router';
import { User } from '../../shared/models/user';
import { RegisterModel } from './shared/models/register.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpAuth = inject(HttpAuth);
  private router = inject(Router);

  public register(user: RegisterModel) {
    return this.httpAuth.post<string>(`auth/register`, user);
  }
}
