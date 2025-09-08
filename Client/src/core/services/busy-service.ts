import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = signal(0);

  busy() {
    this.busyRequestCount.update(n => n + 1);
  }

  idle() {
    this.busyRequestCount.update(n => Math.max(0, n - 1));
  }
}
