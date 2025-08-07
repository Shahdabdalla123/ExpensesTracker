using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses_App_.Core.Models;


namespace Expenses_App_.Core.Interfaces
{
    public interface IExpensesRepositry
    {
        public Task<List<Expense>> GetAllExpenses(string userid);
        public Task<List<Expense>> SelectExpenseByCategory(string UserId, string Category);
        public Task<Expense> SelectExpenseByid(int id, string userid);
        public Task<bool> CreateExpense(Expense E);
        public Task<bool> UpdateExpense(string userid,int id, Expense E);
        public Task<bool> DeleteExpense(string userid, int id);
    }
}
