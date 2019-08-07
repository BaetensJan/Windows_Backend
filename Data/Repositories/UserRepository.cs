using System.Threading.Tasks;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Windows_Backend.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<User> _users;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Users;
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _users.Include(u => u.Business).ThenInclude(b => b.Events)
                .SingleOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}