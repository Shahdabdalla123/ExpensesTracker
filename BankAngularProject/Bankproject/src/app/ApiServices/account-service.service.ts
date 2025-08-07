 import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../app.config';
import { Router } from '@angular/router';
import { AuthenticationService } from './authentication.service';
  
export interface UserDto {
  Username: string;
  Email:string;
  Password: string;
  ConfirmPassword:string;
  
  
}

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {
    constructor(private http: HttpClient,private router:Router,private auth:AuthenticationService) { }
    private apiUrl = `${API_CONFIG.apiUrl}`;

    register(userData: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}Auth/register`,
      {
        userName: userData.get('Username') as string,
        email: userData.get('Email') as string,
        password: userData.get('Password') as string,
        confirmPassword: userData.get('ConfirmPassword') as string
      }
     );
  }
    login(UserName: string, Password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}Auth/login`, {
      UserName: UserName,
      password: Password
    });
  }
    logout(): void {
    this.auth.clearAuth();  
    this.router.navigate(['/login']);
  }


}
