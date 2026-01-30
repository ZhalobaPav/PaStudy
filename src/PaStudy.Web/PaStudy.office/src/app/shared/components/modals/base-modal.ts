import { inject } from '@angular/core';
import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';

export abstract class BaseModalComponent<TData = any, TResult = any> {
  protected dialogRef = inject(DialogRef<TResult>);
  public data = inject<TData>(DIALOG_DATA);
  close(result?: TResult) {
    this.dialogRef.close(result);
  }
}
