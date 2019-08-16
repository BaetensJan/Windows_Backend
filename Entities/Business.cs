using System.Collections.Generic;
using Windows_Backend.Enums;

namespace Windows_Backend.Entities
{
    public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BusinessType Type { get; set; }
        public string Address { get; set; } //TODO: split if needed
        public List<Event> Events { get; set; }
        public List<Promotion> Promotions { get; set; }
    }
}