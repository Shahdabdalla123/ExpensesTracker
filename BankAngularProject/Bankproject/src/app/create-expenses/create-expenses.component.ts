import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../header/header.component';
import { CreateExpensesDTO, ExpensesService } from '../ApiServices/expenses.service';

@Component({
  selector: 'app-create-expenses',
  standalone: true,
  imports: [CommonModule, HeaderComponent, FormsModule, ReactiveFormsModule],
  templateUrl: './create-expenses.component.html',
  styleUrls: ['./create-expenses.component.css']
})
export class CreateExpensesComponent {
  expenseForm: FormGroup;
  isSubmitting = false;
  message = '';

  constructor(
    private fb: FormBuilder,
    private expensesService: ExpensesService,
    public dialogRef: MatDialogRef<CreateExpensesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.expenseForm = new FormGroup({
      Title: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z ]+$')   
      ]),
      Category: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z ]+$')   
      ]),
      Amount: new FormControl<number>(0, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(1)]
      })
    });

  }

  onSubmit() {
    if (this.expenseForm.valid) {
      this.isSubmitting = true;
      const formValue = this.expenseForm.value;
      const expense: CreateExpensesDTO = {
        title: formValue.Title,
        category: formValue.Category,
        amount: formValue.Amount,
        dateTime: new Date().toISOString()
      };

      this.expensesService.createExpenses(expense).subscribe({
        next: (res) => {
          if (res.success) {
            this.message = 'Expense created successfully!';

            // Wait 2 seconds then close dialog and navigate
            setTimeout(() => {
              this.dialogRef.close(true); // Pass true to indicate success
            }, 800);

          } else {
            this.message = res.message;
          }
        },
        error: (err) => {
          console.error(err);
          this.message = 'Error while creating expense!';
        }
      });
    }
  }


  cancel() {
    this.dialogRef.close(false);
  }
}
