import { computed, inject, signal } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

export class LoaderService {
  private spinner = inject(NgxSpinnerService);
  private busyRequestCount = signal<number>(0);

  busy() {
    this.busyRequestCount.update((count) => count++);
    this.spinner.show();
  }

  idle() {
    this.busyRequestCount.update((count) => count--);
    if (this.busyRequestCount() <= 0) {
      this.busyRequestCount.set(0);
      this.spinner.hide();
    }
  }

  public isLoading = computed(() => {
    return this.busyRequestCount() > 0;
  });
}
