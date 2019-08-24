using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows_Backend.Entities;

namespace Windows_Backend.Interfaces
{
    public interface IPromotionRepository
    {
        Task RemoveMultiple(List<Promotion> promotionList);
        Task RemovePromotion(Promotion removePromotion);
        Task<Promotion> FindById(int id);
        Task SaveChanges();
    }
}
