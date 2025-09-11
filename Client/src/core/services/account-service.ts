import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../../types/user';
import { tap } from 'rxjs/internal/operators/tap';
import { RegisterCreds } from '../../types/registerCreds';
import { LoginCreds } from '../../types/loginCreds';
import { environment } from '../../environments/environment';
import { LikesService } from './likes-service';
import { clearHttpCache } from '../interceptors/loading-interceptor';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private likesService = inject(LikesService);
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  currentUser = signal<User | null>(null);

  register(creds: RegisterCreds) {
    return this.http.post<User>(this.baseUrl + 'account/register', creds).pipe(
      tap(user => {
        this.setCurrentUser(user);
      })
    );
  }

  login(creds: LoginCreds) {
    return this.http.post<User>(this.baseUrl + 'account/login', creds).pipe(
      tap(user => {
        this.setCurrentUser(user);
      })
    );
  }

  logout() {
    localStorage.removeItem('user');
    localStorage.removeItem('filters');
    this.currentUser.set(null);
    this.likesService.clearLikeIds();
    clearHttpCache();
  }

  private setCurrentUser(user: User | undefined): void {
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUser.set(user);
      this.likesService.getLikeIds();
    }
  }
}

