import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../ApiServices/authentication.service';
import { AccountServiceService } from '../ApiServices/account-service.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  errorMes: string = '';

  constructor(
    private router: Router,
    private auth: AuthenticationService,
    private user: AccountServiceService
  ) {}

  MyForm = new FormGroup(
    {
      userName: new FormControl('', [
        Validators.required,
        Validators.pattern(/^[a-zA-Z]+(?: [a-zA-Z]+)*$/)
      ]),
      Email: new FormControl('', [
        Validators.required,
        Validators.pattern(/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/)
      ]),
      pass: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(100),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#]).+$/)
      ]),
      confirmPassword: new FormControl('', Validators.required)
    },
    { validators: RegisterComponent.passwordMatchValidator }
  );

static passwordMatchValidator(control: AbstractControl) {
  const passwordControl = control.get('pass');
  const confirmControl = control.get('confirmPassword');

  if (!confirmControl) return null;

   if (confirmControl.errors && confirmControl.errors['required']) {
    return null;
  }

  const password = passwordControl?.value;
  const confirmPassword = confirmControl.value;

  if (password !== confirmPassword) {
    confirmControl.setErrors({ mismatch: true });
  } else {
     if (confirmControl.hasError('mismatch')) {
      confirmControl.setErrors(null);
    }
  }
  return null;
}

  showPassword: boolean = false;

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
    showConfirmPassword: boolean = false;

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }
  onSubmit() {
    if (this.MyForm.invalid) {
      this.errorMes = 'Please fill in all required fields correctly.';
      return;
    }

    const formData = new FormData();
    formData.append('Username', this.MyForm.value.userName!);
    formData.append('Email', this.MyForm.value.Email!);
    formData.append('Password', this.MyForm.value.pass!);
    formData.append('ConfirmPassword', this.MyForm.value.confirmPassword!);

    this.user.register(formData).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.auth.setAuthState(response.token);
          this.auth.setUsername(response.username);
          alert('Registration Successful');
          this.router.navigate(['/login']);
        } else {
          this.MyForm.reset();
          setTimeout(() => {
            alert('Registration failed. Please try again.');
          }, 10);
        }
      },
      error: (err) => {
        alert('An Error occurred. Try again later.');
        console.error('Registration error', err);
      }
    });
  }
}
