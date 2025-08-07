using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Expenses_App.Application.CQRS.Queries;
using Expenses_App_.Core.DTOS.ExpensesDTOS;
using Expenses_App_.Core.Interfaces;
using MediatR;

namespace Expenses_App.Application.CQRS.Handler
{
    public class GetExpensesByIdHandler: IRequestHandler<GetExpensesByIdQuery, ExpensesDTO>
    {
        private readonly IExpensesRepositry _repository;
        private readonly IMapper _mapper;

        public GetExpensesByIdHandler(IExpensesRepositry repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ExpensesDTO> Handle(GetExpensesByIdQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _repository.SelectExpenseByid(request.Id,request.userid);
            return _mapper.Map<ExpensesDTO>(expenses);
        }
    }
}
