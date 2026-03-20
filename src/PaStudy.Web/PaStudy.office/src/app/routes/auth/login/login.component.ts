import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { catchError, of, take, tap, throwError } from 'rxjs';
import { LoginResponse } from '../shared/models/login.model';
import { Router } from '@angular/router';
import { StorageService } from '../../../shared/services/storage.service';
import { TOKEN_KEY } from '../../../shared/contsants/base.constants';
import { NotificationService } from '../../../shared/services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  private formBuilder: FormBuilder = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private notificationService = inject(NotificationService);
  loginForm!: FormGroup;

  ngOnInit(): void {
    this.initFormSubsriptions();
  }

  private initFormSubsriptions(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  public login() {
    const { email, password } = this.loginForm.value;

    this.authService
      .login({ email, password })
      .pipe(
        tap((response: LoginResponse | null) => {
          if (response?.succeeded && response?.token) {
            StorageService.setItem(TOKEN_KEY, response.token);
            this.router.navigate(['/courses']);
          } else {
            this.notificationService.error(
              'Неправильний логін або пароль',
              'помилка',
            );
            StorageService.removeItem(TOKEN_KEY);
          }
        }),
        catchError((err) => of(null)),
        take(1),
      )
      .subscribe();
  }
}
