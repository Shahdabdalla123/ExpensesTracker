using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expenses_App_.Core.DTOS.AuthDTOS;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthenticationService : IAuth
{
    private readonly UserManager<Appuser> _userManager;
    private readonly IConfiguration _config;

    public AuthenticationService(UserManager<Appuser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<AuthResponseDTO> Register(UserRegisterDTO userDTO)
    {
        if (userDTO.Password != userDTO.ConfirmPassword)
        {
            return new AuthResponseDTO
            {
                IsSuccess = false,
                Errors = new List<string> { "Password and Confirm Password do not match." }
            };
        }

        var existingUser = await _userManager.FindByNameAsync(userDTO.UserName);
        if (existingUser != null)
        {
            return new AuthResponseDTO
            {
                IsSuccess = false,
                Errors = new List<string> { "Username already exists." }
            };
        }

        var existingEmail = await _userManager.FindByEmailAsync(userDTO.Email);
        if (existingEmail != null)
        {
            return new AuthResponseDTO
            {
                IsSuccess = false,
                Errors = new List<string> { "Email already exists." }
            };
        }

        var user = new Appuser
        {
            UserName = userDTO.UserName,
            Email = userDTO.Email
        };

        var result = await _userManager.CreateAsync(user, userDTO.Password);
        if (!result.Succeeded)
        {
            return new AuthResponseDTO
            {
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description ?? "Unknown error").ToList()
            };
        }

        var token = await GenerateJwtToken(user);
        return new AuthResponseDTO
        {
            Token = token,
            Username = user.UserName,
            IsSuccess = true
        };
    }


    public async Task<AuthResponseDTO> Login(UserLoginDTO userDTO)
    {
        var user = await _userManager.FindByNameAsync(userDTO.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userDTO.Password))
        {
            return new AuthResponseDTO
            {
                IsSuccess = false,
                Errors = new List<string> { "Invalid username or password" }
            };
        }

        var token = await GenerateJwtToken(user);
        return new AuthResponseDTO
        {
            Token = token,
            Username = user.UserName,
            IsSuccess = true
        };
    }



    public async Task<string> GenerateJwtToken(Appuser user)
    {
        var jwtSettings = _config.GetSection("Jwt");

        // Extract values and check for null or empty strings
        var keyString = jwtSettings["Key"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        if (string.IsNullOrEmpty(keyString))
        {
            throw new Exception("JWT Key is missing in configuration.");
        }
        if (string.IsNullOrEmpty(issuer))
        {
            throw new Exception("JWT Issuer is missing in configuration.");
        }
        if (string.IsNullOrEmpty(audience))
        {
            throw new Exception("JWT Audience is missing in configuration.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
           new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? ""),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(ClaimTypes.NameIdentifier, user.Id ?? ""),
          new Claim(ClaimTypes.Name, user.UserName ?? ""),
    // Add these standard claims
         new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
         new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "")
            };

        var token = new JwtSecurityToken(
               _config["Jwt:Issuer"],
               _config["Jwt:Audience"],
               claims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

 

}
