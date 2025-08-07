using System;
using System.Collections.Generic;

namespace Expenses_App_.Core.Models;

public partial class Expense
{
    public Expense()
    {
        ExpensesDate = DateTime.Now;
    }
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime ExpensesDate { get; set; }

    public string UserId { get; set; }

    public virtual AspNetUser? User { get; set; }
}
