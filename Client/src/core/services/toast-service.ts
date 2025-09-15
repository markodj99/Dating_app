import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor() {
    this.createToastContainer();
  }

  private createToastContainer(): void {
    if (!document.getElementById('toast-container')) {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end z-50';
      document.body.appendChild(container);
    }
  }

  private createToeastElement(message: string, alertClass: string, duratiom = 5000) {
    const toastContainer = document.getElementById('toast-container');
    if (!toastContainer) return;

    const toast = document.createElement('div');
    toast.classList.add('alert', alertClass, 'shadow-lg');

    toast.innerHTML = `
      <span>${message}</span>
      <button class="ml-4 btn btn-sm btn-ghost">X</button>`;

    toast.querySelector('button')?.addEventListener('click', () => {
      toast.remove();
    });

    toastContainer.append(toast);

    setTimeout(() => {
      if (toastContainer.contains(toast)) {
        toast.remove();
      }
    }, duratiom);
  }

  success(message: string, duration?: number): void {
    this.createToeastElement(message, 'alert-success', duration);
  }

  error(message: string, duration?: number): void {
    this.createToeastElement(message, 'alert-error', duration);
  }

  warning(message: string, duration?: number): void {
    this.createToeastElement(message, 'alert-warning', duration);
  }

  info(message: string, duration?: number): void {
    this.createToeastElement(message, 'alert-info', duration);
  }
}
