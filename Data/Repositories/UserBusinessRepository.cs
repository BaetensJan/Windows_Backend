using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;

namespace Windows_Backend.Data.Repositories
{
    public class UserBusinessRepository: IUserBusinessRepository
    {
        private ApplicationDbContext _dbcontext;
        private DbSet<UserBusiness> _userBusiness;
        private DbSet<User> _user;

        public UserBusinessRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
            _userBusiness = dbContext.UserBusiness;
        }
        public Task<List<UserBusiness>> FindByUserId(string userId)
        {
            return _userBusiness.Where(x => x.UserId == userId).ToListAsync();
        }



    }
}
