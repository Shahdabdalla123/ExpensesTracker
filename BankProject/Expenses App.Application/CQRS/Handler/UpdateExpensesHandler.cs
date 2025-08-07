using MediatR;
using AutoMapper;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.Models;
using Expenses_App.Application.CQRS.Command;

public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, bool>
{
    private readonly IExpensesRepositry _repository;
    private readonly IMapper _mapper;

    public UpdateExpenseHandler(IExpensesRepositry repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
         var expenseEntity = _mapper.Map<Expense>(request.ExpenseDto);
         return await _repository.UpdateExpense(request.userid, request.Id, expenseEntity);
    }
}
