import {
  HttpClient,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, map, Observable } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class HttpAuth {
  private readonly router = inject(Router);
  private readonly http = inject(HttpClient);

  public get<T>(url: string): Observable<T> {
    return this.http
      .get<T>(url)
      .pipe(catchError((error: HttpErrorResponse) => this.handleError(error)));
  }

  public post<T>(url: string, data: any): Observable<T> {
    return this.http
      .post<T>(url, data)
      .pipe(catchError((error: HttpErrorResponse) => this.handleError(error)));
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 401) {
      localStorage.clear();
      this.router.navigate(['login']);
    }

    return EMPTY;
  }
}
