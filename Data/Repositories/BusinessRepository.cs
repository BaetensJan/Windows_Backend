using System.Collections.Generic;
using System.Threading.Tasks;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Windows_Backend.Data.Repositories
{
    public class BusinessRepository: IBusinessRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Business> _businesses;
        
        public BusinessRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _businesses = dbContext.Businesses;
        }

        
        public Task<List<Business>> All()
        {
            return _businesses.ToListAsync();
        }
    }
}