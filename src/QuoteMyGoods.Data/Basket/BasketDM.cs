using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Data.Basket
{
    public class BasketDM
    {
        public Dictionary<int, BasketItem> Basket { get; set; }
        public int BasketCount { get; set; }

        public BasketDM()
        {
            Basket = new Dictionary<int, BasketItem>();
            BasketCount = 0;
        }
    }
}
