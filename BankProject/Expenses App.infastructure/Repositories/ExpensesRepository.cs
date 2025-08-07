using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankProject.Models;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses_App.infastructure.Repositories
{
    public class ExpensesRepository : IExpensesRepositry
    {
        public BankprojectContext _DBcontext { get; }
        public IMapper _mapper;

        public ExpensesRepository(BankprojectContext context ,IMapper mapper)
        {
            _DBcontext = context;
            _mapper = mapper;

        }

        public async Task<bool> CreateExpense(Expense E)
        {
            if (E == null)
                return false;

            await _DBcontext.Expenses.AddAsync(E);
            return await _DBcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteExpense(string userid, int id)
        {
            var existing = await SelectExpenseByid(id, userid);

            if (existing == null)
                throw new UnauthorizedAccessException("You are not allowed to Delete this expense.");

            _DBcontext.Expenses.Remove(existing);
            return await _DBcontext.SaveChangesAsync() > 0;
        }

        public async Task<List<Expense>> GetAllExpenses(string userid)
        {
            return await _DBcontext.Expenses
                .Where(e => e.UserId == userid)
                .ToListAsync();
        }

        public async Task<List<Expense>> SelectExpenseByCategory(string userid,string Category)
        {
            return await _DBcontext.Expenses
            .Where(e => e.Category == Category && e.UserId == userid).ToListAsync();
        }

        public async Task<Expense> SelectExpenseByid(int id, string userid)
        {
            return await _DBcontext.Expenses
            .Where(e => e.Id == id && e.UserId==userid).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateExpense(string userid,int id,Expense updatedExpense)
        {
            var existing = await SelectExpenseByid(id,userid);

            if (existing == null)
                throw new UnauthorizedAccessException("You are not allowed to modify this expense."); 


            existing.Title = updatedExpense.Title;
            existing.Category = updatedExpense.Category;
            existing.Amount = updatedExpense.Amount;
            existing.ExpensesDate=DateTime.Now;
            return await _DBcontext.SaveChangesAsync() > 0;
        }

    }
}
