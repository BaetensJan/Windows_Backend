using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows_Backend.Enums;

namespace Windows_Backend.DTO
{
    public class PromotionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; }
        public string StartAndEndDate { get; set; }
        public string Description { get; set; }
    }
}
