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
            return await _users.Include(x => x.Subscribers).Include(u => u.Business).ThenInclude(b => b.Events).Include(x => x.Business).ThenInclude(y => y.Promotions)
                .SingleOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async void SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async void RemoveUserBusinessForUserAsync(string email, int itemInList)
        {
            var user = _users.Include(x => x.Business).Include(f => f.Subscribers).FirstOrDefaultAsync(x => x.Email == email);
            user.Result.Subscribers.RemoveAt(itemInList);
            await _dbContext.SaveChangesAsync();
        }
    }
}