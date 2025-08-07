using Expenses_App_.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BankProject.Models
{
    public class AppDbcontext : IdentityDbContext<Appuser>
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appuser>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }

    }
}
