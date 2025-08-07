using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses_App_.Core.DTOS.ExpensesDTOS;
using MediatR;

namespace Expenses_App.Application.CQRS.Queries
{
    public record GetAllExpensesQuery(string UserId) : IRequest<List<ExpensesDTO>>;

}
