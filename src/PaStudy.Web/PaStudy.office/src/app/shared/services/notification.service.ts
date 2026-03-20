import { inject, Injectable } from '@angular/core';
import { IndividualConfig, ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private toastr = inject(ToastrService);
  private readonly defaultConfig: Partial<IndividualConfig> = {
    timeOut: 4000,
    positionClass: 'toast-top-right',
    closeButton: true,
    progressBar: true,
  };

  public success(message: string, title = 'Успіх') {
    this.toastr.success(message, title, this.defaultConfig);
  }

  public error(message: string, title = 'Помилка') {
    this.toastr.error(message, title, {
      ...this.defaultConfig,
      timeOut: 6000,
    });
  }
}
