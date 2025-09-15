import { Component, inject, output, signal } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { RegisterCreds } from '../../../types/registerCreds';
import { AccountService } from '../../../core/services/account-service';
import { TextInput } from "../../../shared/text-input/text-input";
import { ToastService } from '../../../core/services/toast-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, TextInput],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register  {
  //usersFromHomeComponent = input.required<User[]>(); parent->child komunikacija
  cancelRegister = output<boolean>(); //child->parent komunikacija
  protected creds: RegisterCreds = {} as RegisterCreds;
  private router = inject(Router);
  private accountService = inject(AccountService);
  private toastService = inject(ToastService);
  protected credentialsForm: FormGroup;
  protected profileForm: FormGroup;
  private fb = inject(FormBuilder);
  protected currentStep = signal(1);
  protected validationErrors = signal<string[]>([]);

  constructor() {
    this.credentialsForm = this.fb.group({
      userName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20), this.noWhitespaceValidator]],
      email: ['', [Validators.required, Validators.minLength(4), Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20), this.matchValue('password')]]
    });
    this.credentialsForm.controls['password'].valueChanges.subscribe({
      next: () => this.credentialsForm.controls['confirmPassword'].updateValueAndValidity()
    });

    this.profileForm = this.fb.group({
      gender: ['male', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      city: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20)]],
      country: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20)]]
    });
  }

  noWhitespaceValidator(control: FormControl): ValidationErrors | null {
    const isWhitespace = (control.value || '').indexOf(' ') >= 0;
    return isWhitespace ? { whitespace: true } : null;
  }

  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const parent = control.parent as FormGroup;
      if (!parent) return null;

      const matchValue = parent.get(matchTo)?.value;
      return control.value === matchValue ? null : { passwordMismatch: true }
    };
  }

  nextStep() {
    if (this.credentialsForm.valid) {
      this.currentStep.update(n => n + 1);
    }
  }

  prevStep() {
    this.currentStep.update(n => n - 1);
  }

  getMaxDate() {
    const today = new Date();
    today.setFullYear(today.getFullYear() - 18);
    return today.toISOString().split('T')[0];
  }

  register(): void {
    if (!this.profileForm.valid || !this.credentialsForm.valid) {
      this.toastService.error('Please fill in all required fields correctly');
      return;
    }
    const formData = { ...this.credentialsForm.value, ...this.profileForm.value };

    this.accountService.register(formData).subscribe({
      next: () => {
        this.toastService.success('Registration successful');
        this.router.navigateByUrl('/members');
      },
      error: err => {
        this.toastService.error(err.error)
        this.validationErrors.set(err);
      }
    });
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }
}