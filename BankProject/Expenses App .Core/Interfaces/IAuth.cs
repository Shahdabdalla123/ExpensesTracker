using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses_App_.Core.DTOS.AuthDTOS;
using Expenses_App_.Core.Models;
namespace Expenses_App_.Core.Interfaces
{
    public interface IAuth
    {
        public Task<AuthResponseDTO> Register(UserRegisterDTO userDTO);
        public Task<AuthResponseDTO> Login(UserLoginDTO userDTO);
        public Task<string> GenerateJwtToken(Appuser user);


    }
}
