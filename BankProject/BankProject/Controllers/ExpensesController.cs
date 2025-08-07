using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BankProject.Response;
using Expenses_App.Application.CQRS.Command;
using Expenses_App.Application.CQRS.Queries;
using Expenses_App_.Core.DTOS.ExpensesDTOS;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly HttpClient _httpClient;
        public ExpensesController(IMediator mediator, HttpClient httpClient)
        {
            _mediator = mediator;
            _httpClient = httpClient;

        }
        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(new
            {
                message = "Authenticated!",
                user = User.Identity?.Name
            });
        }
        [Authorize]
        [HttpGet("GetAllExpenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You Are Not Allowed." }));

                }
                var result = await _mediator.Send(new GetAllExpensesQuery(userId));
                if(result==null)
                {
                    return NotFound(ResponseTemplate<object>.ErrorMsg(new List<string> { "No expenses found" }));
                }
                return Ok(ResponseTemplate<List<ExpensesDTO>>.SuccessMsg(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseTemplate<object>.ErrorMsg(new List<string> { ex.Message }));

            }
        }

        [Authorize]
        [HttpGet("GetAllExpensesByCategory/{category}")]
        public async Task<IActionResult> GetExpensesByCategory(string category)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You Are Not Allowed." }));

                }
                if (string.IsNullOrWhiteSpace(category))
                    return BadRequest(new { message = "Category is required" });

                var result = await _mediator.Send(new GetExpensesByCategoryQuery(userId, category));

                if (result == null || result.Count == 0)
                {
                    return NotFound(ResponseTemplate<object>.ErrorMsg(new List<string> { $"No expenses found in category '{category}'" }));

                }

                return Ok(ResponseTemplate<List<ExpensesDTO>>.SuccessMsg(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseTemplate<object>.ErrorMsg(new List<string> { ex.Message }));

            }
        }

        [Authorize]
        [HttpGet("GetAllExpensesByID/{id}")]
        public async Task<IActionResult> GetExpensesByID(int id)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You Are Not Allowed." }));

                }
            

                var result = await _mediator.Send(new GetExpensesByIdQuery(id, userId));

                if (result == null)
                {
                    return NotFound(ResponseTemplate<object>.ErrorMsg(new List<string> { $"No expenses found in ID '{id}'" }));

                }

                return Ok(ResponseTemplate<ExpensesDTO>.SuccessMsg(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseTemplate<object>.ErrorMsg(new List<string> { ex.Message }));

            }
        }



        [Authorize]
        [HttpPost("CreateExpenses")]

        public async Task<IActionResult> CreateExpense(CreateExpensesDTO DTO)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You Are Not Allowed." }));

                }
                var id = await _mediator.Send(new CreateExpenseCommand(userId,DTO));
                if(id==0)
                {
                    var response = ResponseTemplate<int>.ErrorMsg(new List<string> {"Error While Creating Expenses" });
                    return BadRequest(response);
                }
                return Ok(ResponseTemplate<int>.SuccessMsg(id , "Expenses Created SuccessFully"));

             }
            catch (Exception ex)
            {
                return BadRequest(ResponseTemplate<object>.ErrorMsg(new List<string> { ex.Message }));
            }
        }
        [Authorize]
        [HttpPut("UpdateExpenses/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, UpdateDTO dto)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You Are Not Allowed." }));
                }
                var result = await _mediator.Send(new UpdateExpenseCommand(userId,id, dto));
                if (!result)
                {
                    return BadRequest(ResponseTemplate<bool>.ErrorMsg(new List<string> { "Update Failed or You are Not allowed." }));
                }
                return Ok(ResponseTemplate<bool>.SuccessMsg(true, "Expenses Updated SuccessFully"));
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You are not allowed to modify this expense" }));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseTemplate<object>.ErrorMsg(new List<string> { ex.Message }));
            }
        }

        [Authorize]
        [HttpDelete("DeleteExpenses/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You Are Not Allowed." }));
                }
                var result = await _mediator.Send(new DeleteExpenseCommand(userId,id));
                if (!result)
                {
                    return BadRequest(ResponseTemplate<bool>.ErrorMsg(new List<string> { "Delete Failed." }));
                };
                return Ok(ResponseTemplate<bool>.SuccessMsg(true, "Expenses Deleted SuccessFully"));
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest(ResponseTemplate<string>.ErrorMsg(new List<string> { "You are not allowed to modify this expense" }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", details = ex.Message });
            }
        }

    }
}
