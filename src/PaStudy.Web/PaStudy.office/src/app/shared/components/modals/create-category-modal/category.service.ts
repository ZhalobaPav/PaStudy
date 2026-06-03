import { inject, Injectable } from '@angular/core';
import { HttpAuth } from '../../../../core/services/http-auth';
import { Observable } from 'rxjs';
import { Category } from '../../../models/category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private httpAuth = inject(HttpAuth);

  public createCategory(category: { title: string }): Observable<unknown> {
    return this.httpAuth.post('category', category);
  }

  public getCategories(): Observable<Category[]> {
    return this.httpAuth.get('category');
  }
}
