using System.Collections.Generic;
using Windows_Backend.DTO;
using Windows_Backend.Entities;

namespace Windows_Backend.Interfaces
{
    public interface IEventRepository
    {
        void RemoveMultiple(List<Event> evList);
        void RemoveEvent(Event removeEvent);
    }
}