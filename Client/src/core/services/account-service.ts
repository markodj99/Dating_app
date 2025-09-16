import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../../types/user';
import { tap } from 'rxjs/internal/operators/tap';
import { RegisterCreds } from '../../types/registerCreds';
import { LoginCreds } from '../../types/loginCreds';
import { environment } from '../../environments/environment';
import { LikesService } from './likes-service';
import { clearHttpCache } from '../interceptors/loading-interceptor';
import { PresenceService } from './presence-service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private likesService = inject(LikesService);
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private presenceService = inject(PresenceService);

  currentUser = signal<User | null>(null);

  register(creds: RegisterCreds) {
    return this.http.post<User>(this.baseUrl + 'account/register', creds, { withCredentials: true }).pipe(
      tap(user => {
        this.setCurrentUser(user);
        this.startTokenRefreshInterval();
      })
    );
  }

  login(creds: LoginCreds) {
    return this.http.post<User>(this.baseUrl + 'account/login', creds, { withCredentials: true }).pipe(
      tap(user => {
        this.setCurrentUser(user);
        this.startTokenRefreshInterval();
      })
    );
  }

  refreshToken() {
    return this.http.post<User>(this.baseUrl + 'account/refresh-token', {}, { withCredentials: true });
  }

  startTokenRefreshInterval() {
    setInterval(() => {
      console.log("USAO USAO USAO");
      this.http.post<User>(this.baseUrl + 'account/refresh-token', {}, { withCredentials: true }).subscribe({
        next: user => this.setCurrentUser(user),
        error: () => this.logout()
      });
    }, 6 * 60 * 1000);
  }

  logout() {
    localStorage.removeItem('filters');
    this.currentUser.set(null);
    this.likesService.clearLikeIds();
    clearHttpCache();
    this.presenceService.stopHubConnection();
  }

  setCurrentUser(user: User) {
    user.roles = this.getRolesFromToken(user);
    this.currentUser.set(user);
    this.likesService.getLikeIds();

    if (!this.presenceService.isConnected()) this.presenceService.createHubConnection(user);
  }

  private getRolesFromToken(user: User): string[] {
    const payload = user.token.split('.')[1];
    const decoded = atob(payload);

    const jsonPayload = JSON.parse(decoded);
    return Array.isArray(jsonPayload.role) ? jsonPayload.role : [jsonPayload.role];
  }
}

