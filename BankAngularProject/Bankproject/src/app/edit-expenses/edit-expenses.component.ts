import { Component, Inject, OnInit } from '@angular/core';
import { HeaderComponent } from '../header/header.component';
import { CommonModule } from '@angular/common';
import { Validators, ReactiveFormsModule, FormsModule, FormControl, FormGroup } from '@angular/forms';
import { ExpensesService, updateExpensesDTO } from '../ApiServices/expenses.service';
import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-expenses',
  standalone: true,
  imports: [HeaderComponent, CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './edit-expenses.component.html',
  styleUrls: ['./edit-expenses.component.css']
})
export class EditExpensesComponent implements OnInit {

  isSubmitting = false;
  message = '';
  expenseId!: number;
  EditexpenseForm: FormGroup;


  constructor(
    private route: ActivatedRoute,
    private expenseService: ExpensesService,
    public dialogRef: MatDialogRef<EditExpensesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.EditexpenseForm = new FormGroup({
      Amount: new FormControl<number>(0, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(1)]
      }),
      Title: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z ]+$')   
      ]),
      Category: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z ]+$')   
      ]),
    });
  }

 ngOnInit(): void {
   this.expenseId = this.data.expenseId;

  if (this.expenseId) {
    this.expenseService.getExpenseById(this.expenseId).subscribe(response => {
      const expense = response.data;
      this.EditexpenseForm.patchValue({
        Title: expense.title,
        Category: expense.category,
        Amount: expense.amount
      });
    });
  }
}

  onSubmit(): void {
    if (this.EditexpenseForm.invalid) return;

    this.isSubmitting = true;
    const expenseData: updateExpensesDTO = {
      title: this.EditexpenseForm.value.Title ?? '',
      category: this.EditexpenseForm.value.Category ?? '',
      amount: this.EditexpenseForm.value.Amount ?? 0
    };

    this.expenseService.updateExpense(this.expenseId, expenseData).subscribe({
      next: (response) => {
        if (response.success) {
          this.message = 'Expense updated successfully!';
           setTimeout(() => {
            this.dialogRef.close(true);  
          }, 800);
        } else {
          this.message = response.message;
        }
       },
      error: (err) => {
        console.error(err);
        this.message = 'Error updating expense';
       }
    });
  }
  cancel() {
    this.dialogRef.close(false);
  }
}
