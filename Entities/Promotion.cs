using System;
using Windows_Backend.Enums;

namespace Windows_Backend.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public DateTime Creation { get; set; }

        public DateTimeOffset ConvertStringToDateTimeOffset(string dateTime)
        {
            return DateTimeOffset.Parse(dateTime);
        }
    }
}