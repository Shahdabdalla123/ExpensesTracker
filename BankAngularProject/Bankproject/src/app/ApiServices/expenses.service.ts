import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_CONFIG } from '../app.config';

export interface ExpensesDTo {
  id: number;
  title: string;
  category: string;
  amount: number;
  expensesDate: string;
  userId: string;
}
export interface CreateExpensesDTO {
  title: string;
  category: string;
  amount: number;
  dateTime: string;


}
export interface updateExpensesDTO {
  title: string;
  category: string;
  amount: number;
 
}
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}
@Injectable({
  providedIn: 'root'
})
export class ExpensesService {
  private apiUrl = `${API_CONFIG.apiUrl}`;
  constructor(private http: HttpClient) { }
  getAllExpenses() {
    return this.http.get<ApiResponse<ExpensesDTo[]>>(`${this.apiUrl}api/Expenses/GetAllExpenses`);
  }
  getAllExpensesByCategory(category:string) {
    return this.http.get<ApiResponse<ExpensesDTo[]>>(`${this.apiUrl}api/Expenses/GetAllExpensesByCategory/${category}`);
  }
  getExpenseById(id: number) {
    return this.http.get<ApiResponse<ExpensesDTo>>(`${this.apiUrl}api/Expenses/GetAllExpensesByID/${id}`);
  }
  createExpenses(expense: CreateExpensesDTO) {
    return this.http.post<ApiResponse<number>>(
      `${this.apiUrl}api/Expenses/CreateExpenses`,
      expense
    );
  }
  DeleteExpenses(id: number) {
    return this.http.delete<ApiResponse<boolean>>(
      `${this.apiUrl}api/Expenses/DeleteExpenses/${id}`
    );
  }
updateExpense(id: number, expense: updateExpensesDTO) {
  return this.http.put<ApiResponse<boolean>>(
    `${this.apiUrl}api/Expenses/UpdateExpenses/${id}`,
    expense
  );
}

}
