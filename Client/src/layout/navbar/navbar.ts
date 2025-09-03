import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-navbar',
  imports: [FormsModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  protected creds:any = {};
  private accountService = inject(AccountService);

  login() {
    this.accountService.login(this.creds).subscribe({
      next: response => console.log(response),
      error: error => alert(error.message)
    });
  }
}
