import { inject } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

export class LoaderService {
  private spinner = inject(NgxSpinnerService);
  private busyRequestCount = 0;

  busy() {
    this.busyRequestCount++;
    this.spinner.show();
  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinner.hide();
    }
  }
}
