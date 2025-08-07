using MediatR;
using AutoMapper;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.DTOS.ExpensesDTOS;
using Expenses_App.Application.CQRS.Queries;

public class GetExpensesByCategoryHandler
    : IRequestHandler<GetExpensesByCategoryQuery, List<ExpensesDTO>>
{
    private readonly IExpensesRepositry _repository;
    private readonly IMapper _mapper;

    public GetExpensesByCategoryHandler(IExpensesRepositry repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ExpensesDTO>> Handle(GetExpensesByCategoryQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.SelectExpenseByCategory(request.UserId, request.Category);
        return _mapper.Map<List<ExpensesDTO>>(expenses);
    }
}
