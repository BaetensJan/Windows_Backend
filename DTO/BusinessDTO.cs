using Windows_Backend.Enums;

namespace Windows_Backend.DTO
{
    public class BusinessDTO
    {
        public string Name { get; set; }
        public BusinessType Type { get; set; }
        public string Address { get; set; } //TODO: split if needed
    }
}