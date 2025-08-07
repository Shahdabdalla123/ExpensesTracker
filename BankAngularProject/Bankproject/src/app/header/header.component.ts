import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { AuthenticationService } from '../ApiServices/authentication.service';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';
import { CreateExpensesComponent } from '../create-expenses/create-expenses.component';
import { ExpensesService } from '../ApiServices/expenses.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  username: string | null = null;
  isWelcomePage = false;

  constructor(private auth: AuthenticationService, private router: Router
,

  ) {}

  ngOnInit(): void {
    this.username = this.auth.getUsername();
    this.isWelcomePage = this.router.url === '/welcome';

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.isWelcomePage = event.urlAfterRedirects === '/welcome';
      });
  }

 

  logout() {
    this.auth.clearAuth();
    this.router.navigate(['/login']);
  }
   
  
}
