using System.Collections.Generic;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Windows_Backend.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private ApplicationDbContext _dbcontext;
        private DbSet<Event> _events;

        public EventRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
            _events = dbContext.Events;
        }

        public async void RemoveMultiple(List<Event> evList)
        {
            _events.RemoveRange(evList);
            await _dbcontext.SaveChangesAsync();
        }
        public void RemoveEvent(Event removeEvent)
        {
            var removeEventById = _events.Where(x => x.Id == removeEvent.Id).First();
            _events.Remove(removeEventById);
            _dbcontext.SaveChanges();
        }
    }
}