import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../ApiServices/authentication.service';
import { HeaderComponent } from "../header/header.component";
import { ExpensesService, ExpensesDTo } from '../ApiServices/expenses.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CreateExpensesComponent } from '../create-expenses/create-expenses.component';
import { EditExpensesComponent } from '../edit-expenses/edit-expenses.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HeaderComponent, CommonModule, FormsModule, MatDialogModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  expenses: ExpensesDTo[] = [];
  searchCategory: string = '';
  TotalExpenses: number = 0

  constructor(
    public auth: AuthenticationService,
    private expensesService: ExpensesService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadExpenses();
  }

  loadExpenses() {
    this.expensesService.getAllExpenses().subscribe({
      next: (res) => {
        this.expenses = res.data;
        this.TotalExpenses = res.data.length;
        console.log('Expenses:', this.expenses);
      },
      error: (err) => console.error('Error fetching expenses', err)
    });
  }

  navigateToCreate() {
    const dialogRef = this.dialog.open(CreateExpensesComponent, {
      width: '900px',
      disableClose: true,
      data: {}
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadExpenses();
      }
    });
  }

  deleteExpense(id: number) {
    if (confirm('Are you sure you want to delete this expense?')) {
      this.expensesService.DeleteExpenses(id).subscribe({
        next: (res) => {
          if (res.success) {
            this.expenses = this.expenses.filter(exp => exp.id !== id);
            this.TotalExpenses = this.expenses.length;
          } else {
            alert('Failed to delete expense');
          }
        },
        error: (err) => {
          console.error(err);
          alert('Error deleting expense');
        }
      });
    }
  }


  editExpense(id: number) {
    const dialogRef = this.dialog.open(EditExpensesComponent, {
      width: '900px',
      disableClose: true,
      data: { expenseId: id }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadExpenses();
      }
    });
  }
  filteredExpenses() {
    if (this.searchCategory) {
      this.expensesService.getAllExpensesByCategory(this.searchCategory).subscribe({
        next: (res) => {
          if (res.success) {
            this.expenses = res.data;
          } else {
            this.loadExpenses();
          }
        },

      });
    }
    else {
      this.loadExpenses();
    }
  }

  clearSearch() {
    this.searchCategory = '';
    this.loadExpenses();
  }
}
