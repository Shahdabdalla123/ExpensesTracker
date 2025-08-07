using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Expenses_App.Application.CQRS.Command
{
 
    public record DeleteExpenseCommand(string userid,int Id) : IRequest<bool>;
}
