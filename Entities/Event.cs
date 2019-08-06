using Windows_Backend.Enums;

namespace Windows_Backend.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}