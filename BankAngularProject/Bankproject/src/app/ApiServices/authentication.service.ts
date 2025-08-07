import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private userSubject = new BehaviorSubject<{ username: string } | null>(null);
  currentUser$ = this.userSubject.asObservable();

  constructor() {
    const username = localStorage.getItem('username');
    if (username) {
      this.userSubject.next({ username });
    }
  }

  setAuthState(token: string): void {
    localStorage.setItem('jwtToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  clearAuth(): void {
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('username');
    this.userSubject.next(null);  
  }

  setUsername(username: string) {
    localStorage.setItem('username', username);
    this.userSubject.next({ username });  
  }

  getUsername(): string | null {
    return localStorage.getItem('username');
  }

  logout() {
    this.clearAuth();
  }
}
