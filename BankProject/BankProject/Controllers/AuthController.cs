using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expenses_App_.Core.DTOS.AuthDTOS;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;

namespace BankProject.Controllers
{
    [Route("Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO model)
        {
            try
            {
                var response = await _authService.Register(model);
                return Ok(response);   
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            try
            {
                var response = await _authService.Login(model);
                return Ok(response);   
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }


}




