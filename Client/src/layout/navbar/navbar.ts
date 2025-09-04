import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { LoginCreds } from '../../types/user';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';


@Component({
  selector: 'app-navbar',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  protected creds: LoginCreds = {} as LoginCreds;
  protected accountService = inject(AccountService);
  private router = inject(Router);

  login(): void {
    this.accountService.login(this.creds).subscribe({
      next: response => { 
        this.router.navigateByUrl('/users');
        this.creds = {} as LoginCreds;
      },
      error: error => alert(error.message)
    });
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}

