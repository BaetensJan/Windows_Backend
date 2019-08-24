using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;

namespace Windows_Backend.Data.Repositories
{
    public class PromotionRepository: IPromotionRepository
    {
        private ApplicationDbContext _dbcontext;
        private DbSet<Promotion> _promotions;

        public PromotionRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
            _promotions = dbContext.Promotions;
        }

        public async Task RemoveMultiple(List<Promotion> promotionList)
        {
            _promotions.RemoveRange(promotionList);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task RemovePromotion(Promotion removePromotion)
        {
            var removePromotionById = _promotions.Where(x => x.Id == removePromotion.Id).First();
            _promotions.Remove(removePromotionById);
             await _dbcontext.SaveChangesAsync();
        }

        public async Task<Promotion> FindById(int id)
        {
            return await _promotions.SingleOrDefaultAsync(x => x.Id == id);
        }
        public Task SaveChanges()
        {
           return _dbcontext.SaveChangesAsync();
        }
    }
}
