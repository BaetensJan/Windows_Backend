using System.Collections.Generic;
using System.Threading.Tasks;
using Windows_Backend.Entities;

namespace Windows_Backend.Interfaces
{
    public interface IBusinessRepository
    {
        Task<List<Business>> All();
        Task<Business> FindById(int id);
        Task SaveChanges();
    }
}