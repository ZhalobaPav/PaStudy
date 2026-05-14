import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../core/services/http-auth';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { RegisterModel } from './shared/models/register.model';
import { LoginModel, LoginResponse } from './shared/models/login.model';
import { CLAIM_TYPES, TOKEN_KEY } from '../../shared/contsants/base.constants';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserRoleString } from '../../shared/enums/userRole';
import { StorageService } from '../../shared/services/storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpAuth = inject(HttpAuth);
  private router = inject(Router);
  private userRoleSubject = new BehaviorSubject<UserRoleString | null>(null);
  userRole$: Observable<string | null> = this.userRoleSubject.asObservable();

  constructor() {
    this.loadUserFromToken();
  }

  public get isAuthorized() {
    return !!localStorage.getItem(TOKEN_KEY);
  }

  public register(user: RegisterModel) {
    return this.httpAuth.post<string>(`auth/register`, user);
  }

  public googleLogin() {}

  public login(loginModel: LoginModel) {
    return this.httpAuth.post<LoginResponse>(`auth/login`, loginModel);
  }

  public logout() {
    StorageService.removeItem(TOKEN_KEY);
  }

  public loadUserFromToken() {
    const token = localStorage.getItem(TOKEN_KEY);
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const role =
          decoded[CLAIM_TYPES.SHORT_ROLE] || decoded[CLAIM_TYPES.ROLE];
        this.userRoleSubject.next(role);
      } catch (e) {
        this.userRoleSubject.next(null);
      }
    }
  }

  get currentRole(): string | null {
    return this.userRoleSubject.value;
  }

  isTeacher(): boolean {
    return this.currentRole === UserRoleString.Teacher;
  }

  isStudent(): boolean {
    return this.currentRole === UserRoleString.Student;
  }
}
