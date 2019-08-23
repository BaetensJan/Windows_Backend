﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows_Backend.Entities;

namespace Windows_Backend.Interfaces
{
    public interface IPromotionRepository
    {
        void RemoveMultiple(List<Promotion> promotionList);
        void RemovePromotion(Promotion removePromotion);
    }
}
