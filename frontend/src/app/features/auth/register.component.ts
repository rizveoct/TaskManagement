import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'tm-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  template: `
    <div class="auth-layout">
      <form [formGroup]="form" class="auth-card" (ngSubmit)="submit()">
        <h1 class="text-2xl font-semibold">Create your account</h1>
        <p class="text-sm text-foreground/60 mb-6">Invite your team and start collaborating instantly.</p>
        <label class="form-field">
          <span>Full Name</span>
          <input formControlName="name" type="text" placeholder="Alex Johnson" />
        </label>
        <label class="form-field">
          <span>Email</span>
          <input formControlName="email" type="email" placeholder="you@example.com" />
        </label>
        <label class="form-field">
          <span>Password</span>
          <input formControlName="password" type="password" placeholder="••••••••" />
        </label>
        <label class="form-field">
          <span>Confirm Password</span>
          <input formControlName="confirmPassword" type="password" placeholder="••••••••" />
        </label>
        <button class="btn-primary w-full mt-4" type="submit" [disabled]="form.invalid">Create account</button>
        <p class="text-sm text-center text-foreground/60 mt-4">
          Already have an account?
          <a routerLink="/auth/login" class="text-accent">Sign in</a>
        </p>
      </form>
    </div>
  `,
  styles: [
    `
      .auth-layout {
        @apply min-h-screen flex items-center justify-center bg-layer-0;
      }
      .auth-card {
        @apply bg-layer-1 border border-border rounded-2xl shadow-xl px-10 py-12 w-full max-w-md;
      }
      .form-field {
        @apply flex flex-col space-y-2 text-sm mb-4;
      }
      .form-field input {
        @apply px-3 py-2 rounded-lg border border-border bg-layer-2 focus:border-accent focus:ring-2 focus:ring-accent/40 outline-none transition;
      }
      .btn-primary {
        @apply inline-flex items-center justify-center px-4 py-2 rounded-lg bg-accent text-white text-sm font-medium shadow hover:bg-accent/90 transition;
      }
    `,
  ],
  providers: [FormBuilder],
})
export class RegisterComponent {
  readonly form = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required],
  });

  constructor(private readonly fb: FormBuilder) {}

  submit() {
    if (this.form.valid) {
      console.log('Register', this.form.value);
    }
  }
}
