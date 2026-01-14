import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { Router } from '@angular/router';
import { User } from '../../shared/models/user';
import { RegisterModel } from './shared/models/register.model';
import { LoginModel, LoginResponse } from './shared/models/login.model';
import { TOKEN_KEY } from '../../shared/contsants/base.constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpAuth = inject(HttpAuth);
  private router = inject(Router);

  public get isAuthorized() {
    return !!localStorage.getItem(TOKEN_KEY);
  }

  public register(user: RegisterModel) {
    return this.httpAuth.post<string>(`auth/register`, user);
  }

  public login(loginModel: LoginModel) {
    return this.httpAuth.post<LoginResponse>(`auth/login`, loginModel);
  }
}
