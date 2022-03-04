import {Injectable} from '@angular/core';
import {NgxSpinnerService} from "ngx-spinner";

/**
 * service to let us know  if we are loading anything
 * used with the ngx-spinner
 */
@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;

  constructor(private spinnerService: NgxSpinnerService) {
  }

  busy() {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: 'pacman',
      size: 'medium',
      bdColor: 'rgba(255,255,255, .7)',
      color: '#333333',
      fullScreen: true,
    });
  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}
