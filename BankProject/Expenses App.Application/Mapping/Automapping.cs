using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Expenses_App_.Core.DTOS.ExpensesDTOS;
using Expenses_App_.Core.Models;

namespace Expenses_App.Application.Mapping
{
    public  class Automapping :Profile
    {
        public Automapping()
        {
            CreateMap<CreateExpensesDTO, Expense>().ReverseMap();
            CreateMap<UpdateDTO, Expense>().ReverseMap();
            CreateMap<Expense, ExpensesDTO>().ReverseMap();
        }
           
     }
}
