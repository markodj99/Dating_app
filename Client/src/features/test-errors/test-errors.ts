import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css'
})
export class TestErrors {
  private httpClient = inject(HttpClient);
  baseUrl = environment.apiUrl;
  protected validationErrors = signal<string[]>([]);

  get404Error() {
    this.httpClient.get(this.baseUrl + 'faulty/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400Error() {
    this.httpClient.get(this.baseUrl + 'faulty/bad-request').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500Error() {
    this.httpClient.get(this.baseUrl + 'faulty/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get401Error() {
    this.httpClient.get(this.baseUrl + 'faulty/auth').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400ValidationError() {
    this.httpClient.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error => {
        this.validationErrors.set(error);
        console.log(error);
      }
    })
  }
}
