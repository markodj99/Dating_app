import { Component, inject, Input, signal } from '@angular/core';
import { Register } from '../account/register/register';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  //@Input({required: true}) usersFromAppComponent: User[] = []; parent->child komunikacija
  protected accountService = inject(AccountService);
  protected registerMode = signal(false);

  showRegister(value: boolean): void {
    this.registerMode.set(value);
  }
}
