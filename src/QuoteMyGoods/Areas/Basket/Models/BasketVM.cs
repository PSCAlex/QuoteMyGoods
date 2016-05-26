using QuoteMyGoods.Data.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Areas.Basket.Models
{
    public class BasketVM
    {
        public Dictionary<int, BasketItem> Basket { get; set; }
        public int BasketCount { get; set; }
    }
}
