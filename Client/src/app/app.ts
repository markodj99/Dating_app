import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Navbar } from "../layout/navbar/navbar";

@Component({
  selector: 'app-root',
  imports: [Navbar],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title: string = 'Dating App';
  
  protected users = signal<any>([]);

  // constructor(private http: HttpClient) { } stari nacin za DI
  private http = inject(HttpClient); // novi nacin za DI

  async ngOnInit(): Promise<void> {
    this.users.set(await this.getUsers()); // ako se bojimo da ne uradi unsubscribe

    // this.http.get('https://localhost:5001/api/users/all').subscribe({
    //   next: response => this.users.set(response),
    //   error: error => console.log(error),
    //   complete: () => console.log('Request completed') // opcionalno
    // });
  }

  async getUsers(): Promise<Object> {
    try {
      return lastValueFrom(this.http.get('https://localhost:5001/api/users/all'));
  } catch (error) {
      console.log(error)
      throw error;
    }
  }
}
