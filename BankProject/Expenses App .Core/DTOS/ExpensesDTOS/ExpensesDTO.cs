using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses_App_.Core.DTOS.ExpensesDTOS
{
    public class ExpensesDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpensesDate { get; set; }
        public string UserId { get; set; }
    }
}
