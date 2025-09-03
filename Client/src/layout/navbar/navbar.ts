import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { LoginCreds } from '../../types/user';

@Component({
  selector: 'app-navbar',
  imports: [FormsModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  protected creds: LoginCreds = {} as LoginCreds;
  protected accountService = inject(AccountService);

  login(): void {
    this.accountService.login(this.creds).subscribe({
      next: response => { 
        console.log(response);
        this.creds = {} as LoginCreds;
      },
      error: error => alert(error.message)
    });
  }

  logout(): void {
    this.accountService.logout();
  }
}

