import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { LoginCreds } from '../../types/user';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';


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
  private toastService = inject(ToastService);

  login(): void {
    this.accountService.login(this.creds).subscribe({
      next: () => { 
        this.router.navigateByUrl('/users');
        this.toastService.success('Login successful');
        this.creds = {} as LoginCreds;
      },
      error: (err) => {
        const msg: string = typeof err.error === 'string' ? err.error : 'Login failed, please try again';
        this.toastService.error(msg);
      }
    });
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}

