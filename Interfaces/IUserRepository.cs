using System.Threading.Tasks;
using Windows_Backend.Entities;

namespace Windows_Backend.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmail(string email);
        void RemoveUserBusinessForUserAsync(string email, int itemInList);
        void SaveChanges();
    }
}