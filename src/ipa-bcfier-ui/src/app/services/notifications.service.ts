import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  constructor(private toastr: ToastrService) {
  }

  private readonly notificationDurationMs = 7 * 1000; // 7 seconds

  public success(message: string, title?: string): void {
    this.toastr.success(message, title, {
      timeOut: this.notificationDurationMs
    });
  }

  public error(message: string, title?: string): void {
    this.toastr.error(message, title, {
      timeOut: this.notificationDurationMs
    });
  }

  public info(message: string, title?: string, timeOut = 0): void {
    this.toastr.info(message, title, {
      timeOut: timeOut || this.notificationDurationMs
    });
  }

  public warning(message: string, title?: string, timeOut = 0): void {
    this.toastr.warning(message, title, {
      timeOut: timeOut || this.notificationDurationMs
    });
  }
}
