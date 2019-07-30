using Windows_Backend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Windows_Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private IConfiguration Configuration { get; }
        
        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration["ConnectionString"]);
        }
    }
}