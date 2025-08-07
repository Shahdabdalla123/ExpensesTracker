using MediatR;
using AutoMapper;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.DTOS.ExpensesDTOS;
using Expenses_App.Application.CQRS.Queries;

public class GetAllExpensesHandler : IRequestHandler<GetAllExpensesQuery, List<ExpensesDTO>>
{
    private readonly IExpensesRepositry _repository;
    private readonly IMapper _mapper;

    public GetAllExpensesHandler(IExpensesRepositry repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ExpensesDTO>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.GetAllExpenses(request.UserId);
        return _mapper.Map<List<ExpensesDTO>>(expenses);
    }
}
