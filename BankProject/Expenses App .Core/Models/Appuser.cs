using Microsoft.AspNetCore.Identity;

namespace Expenses_App_.Core.Models
{
    public class Appuser : IdentityUser
    {
         public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
