using System.Collections.Generic;
using System.Threading.Tasks;
using Windows_Backend.DTO;
using Windows_Backend.Entities;

namespace Windows_Backend.Interfaces
{
    public interface IEventRepository
    {
        Task RemoveMultiple(List<Event> evList);
        Task RemoveEvent(Event removeEvent);
        Task<Event> FindEventById(int id);
        Task SaveChanges();

    }
}