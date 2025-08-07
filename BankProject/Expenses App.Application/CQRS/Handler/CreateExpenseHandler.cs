using MediatR;
using AutoMapper;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.Models;
using Expenses_App.Application.CQRS.Command;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, int>
{
    private readonly IExpensesRepositry _repository;
    private readonly IMapper _mapper;

    public CreateExpenseHandler(IExpensesRepositry repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = _mapper.Map<Expense>(request.ExpenseDto);
        expense.UserId = request.userid;
        var result = await _repository.CreateExpense(expense);
        return result ? expense.Id : 0;
    }
}
