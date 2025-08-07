using MediatR;
using Expenses_App_.Core.Interfaces;
using Expenses_App.Application.CQRS.Command;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpensesRepositry _repository;

    public DeleteExpenseHandler(IExpensesRepositry repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
         return await _repository.DeleteExpense(request.userid, request.Id);
    }
}
