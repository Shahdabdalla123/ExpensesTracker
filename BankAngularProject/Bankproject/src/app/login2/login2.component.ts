import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { AccountServiceService } from '../ApiServices/account-service.service';
import { AuthenticationService } from '../ApiServices/authentication.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login2.component.html',
  styleUrl: './login2.component.css'
})
export class Login2Component {
  constructor(private router: Router, private accountService: AccountServiceService, private auth: AuthenticationService) {

  }
  MyForm = new FormGroup(
    {

      userName: new FormControl('', [
        Validators.required,
        Validators.pattern(/^[a-zA-Z]+(?: [a-zA-Z]+)*$/)]),
      pass: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(100),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#]).+$/)
      ])

    },

  );
  showPassword: boolean = false;

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
  onSubmit() {
    const userName = this.MyForm.value.userName!;
    const Password = this.MyForm.value.pass!;

    this.accountService.login(userName, Password).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.auth.setAuthState(response.token);
          this.auth.setUsername(response.username);
          this.router.navigate(['/home']);
        } else {
          this.MyForm.reset();

          if (Array.isArray(response.errors)) {
            alert(response.errors.join(', '));
          } else if (typeof response.errors === 'string') {
            alert(response.errors);
          }

        }
      },
      error: (err) => {
        alert('An Error occurred try again later');
        console.error('Login error', err);
      }
    });
  }
}
