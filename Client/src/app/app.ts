import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Navbar } from "../layout/navbar/navbar";
import { AccountService } from '../core/services/account-service';
import { User } from '../types/user';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [Navbar, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title: string = 'Dating App';
  protected router = inject(Router);
  protected users = signal<User[]>([]);

  // constructor(private http: HttpClient) { } stari nacin za DI
  private http = inject(HttpClient); // novi nacin za DI
  private accountService = inject(AccountService);

  async ngOnInit(): Promise<void> {
    this.users.set(await this.getUsers()); // ako se bojimo da ne uradi unsubscribe
    this.setCurrentUser();
    // this.http.get('https://localhost:5001/api/user/all').subscribe({
    //   next: response => this.users.set(response),
    //   error: error => console.log(error),
    //   complete: () => console.log('Request completed') // opcionalno
    // });
  }

  async getUsers(): Promise<User[]> {
    try {
      return lastValueFrom(this.http.get<User[]>('https://localhost:5001/api/user/all'));
  } catch (error) {
      console.log(error)
      throw error;
    }
  }

  setCurrentUser() {

  }
}
