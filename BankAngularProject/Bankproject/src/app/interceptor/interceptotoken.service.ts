import { Injectable } from '@angular/core';
import { AuthenticationService } from '../ApiServices/authentication.service';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { AccountServiceService } from '../ApiServices/account-service.service';

@Injectable({
  providedIn: 'root'
})
@Injectable()
export class InterceptorTokenService implements HttpInterceptor {
  constructor(private auth: AuthenticationService, private router: Router,private account:AccountServiceService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.auth.getToken();
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401) {
          this.account.logout();
          this.router.navigate(['/login'], { queryParams: { message: 'Unauthorized' } });
        }
        return throwError(() => error);
      })
    );
  }
}
