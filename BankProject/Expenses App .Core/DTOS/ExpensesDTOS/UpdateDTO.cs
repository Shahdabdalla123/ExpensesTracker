using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses_App_.Core.DTOS.ExpensesDTOS
{
    public class UpdateDTO
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
      
    }
}
 
