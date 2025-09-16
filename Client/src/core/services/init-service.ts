import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { Observable } from 'rxjs';
import { LikesService } from './likes-service';
import { tap } from 'rxjs/internal/operators/tap';
import { User } from '../../types/user';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private accountService = inject(AccountService);

  init(): Observable<User> {
    return this.accountService.refreshToken().pipe(
      tap(user => {
        if (user) {
          this.accountService.setCurrentUser(user);
          this.accountService.startTokenRefreshInterval();
        }
      })
    );
  }
}
