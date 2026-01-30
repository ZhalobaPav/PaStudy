import { Dialog, DialogConfig, DialogRef } from '@angular/cdk/dialog';
import { inject, Injectable, Type } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private dialog = inject(Dialog);

  public open<T, D = any, R = any>(
    component: Type<T>,
    data: D,
    config: Partial<DialogConfig<D, DialogRef<R, T>>> = {},
  ) {
    return this.dialog.open<R, D, T>(component, {
      ...config,
      data,
      backdropClass: 'modal-backdrop-blur',
      panelClass: 'modal-panel-container',
    });
  }
}
