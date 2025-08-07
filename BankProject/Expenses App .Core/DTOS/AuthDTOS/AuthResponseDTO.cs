using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses_App_.Core.DTOS.AuthDTOS
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
