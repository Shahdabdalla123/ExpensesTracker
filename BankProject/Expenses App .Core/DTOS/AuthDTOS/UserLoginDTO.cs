using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses_App_.Core.DTOS.AuthDTOS
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username Required !!")]

        public string Username { get; set; }
 
        [Required(ErrorMessage = "Password is required !!")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#]).+$",
            ErrorMessage = "Password must contain uppercase, lowercase, digit, and special character.")]
        public string Password { get; set; }
    }
}
