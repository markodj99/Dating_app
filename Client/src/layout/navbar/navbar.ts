import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { LoginCreds } from '../../types/loginCreds';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';
import { themes } from '../theme';
import { BusyService } from '../../core/services/busy-service';


@Component({
  selector: 'app-navbar',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit{
  protected creds: LoginCreds = {} as LoginCreds;
  protected accountService = inject(AccountService);
  private router = inject(Router);
  private toastService = inject(ToastService);
  protected selectedTheme = signal<string>(localStorage.getItem('theme') || 'dark');
  protected themes = themes;
  protected busyService = inject(BusyService);
  protected loading = signal(false);

  ngOnInit(): void {
    document.documentElement.setAttribute('data-theme', this.selectedTheme());
  }

  handleSelectedTheme(theme: string) {
    this.selectedTheme.set(theme);
    localStorage.setItem('theme', theme);
    document.documentElement.setAttribute('data-theme', theme);
    const elem = document.activeElement as HTMLDivElement;
    if (elem) elem.blur();
  }

  handleSelectUserItem() {
    const elem = document.activeElement as HTMLDivElement;
    if (elem) elem.blur();
  }

  login(): void {
    this.loading.set(true);
    this.accountService.login(this.creds).subscribe({
      next: () => { 
        this.router.navigateByUrl('/members');
        this.toastService.success('Login successful');
        this.creds = {} as LoginCreds;
      },
      error: (err) => {
        const msg: string = typeof err.error === 'string' ? err.error : 'Login failed, please try again';
        this.toastService.error(msg);
      },
      complete: () => this.loading.set(false)
    });
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}

