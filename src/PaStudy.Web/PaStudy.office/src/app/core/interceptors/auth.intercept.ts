import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { environment } from '../../../environments/environment.dev';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  public readonly apiUrl = environment.apiPaStudyUrl;

  intercept<T>(
    req: HttpRequest<T>,
    next: HttpHandler
  ): Observable<HttpEvent<T>> {
    if (req.url.includes('assets/')) {
      return next.handle(req);
    }

    const reqClone = req.clone({
      url: `${this.apiUrl}/${req.url}`,
    });

    return next.handle(reqClone).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error(error, 'error');
        if (error.status === 401) {
          this.handleUnauthorizedError();
        }
        return throwError(() => error);
      })
    );
  }

  private handleUnauthorizedError(): void {
    this.router.navigate(['/login']);
  }
}
