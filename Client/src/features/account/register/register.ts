import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds, User } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  //usersFromHomeComponent = input.required<User[]>(); parent->child komunikacija
  cancelRegister = output<boolean>(); //child->parent komunikacija
  protected creds:RegisterCreds = {} as RegisterCreds;
  private accountService = inject(AccountService);

  register():void {
    this.accountService.register(this.creds).subscribe({
      next: user => {
        console.log(user);
        this.cancel();
      },
      error: err => console.log(err.message)
    });
  }

  cancel():void {
    this.cancelRegister.emit(false);
  }
}