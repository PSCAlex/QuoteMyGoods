using System.Collections.Generic;
using QuoteMyGoods.Models;
using QuoteMyGoods.Common;
using Microsoft.AspNet.Http;

namespace QuoteMyGoods.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _context;
        private Basket _basket;

        public BasketService(IHttpContextAccessor context)
        {
            _context = context;
            _basket = new Basket(_context);
        }

        public void AddQuantity(int id)
        {
            _basket.GetItemById(id).AddItem();
            _basket.SaveToSession();
        }

        public void AddToBasket(Product product)
        {
            _basket.AddToBasket(product);
        }

        public void ClearBasket()
        {
            _basket.ClearBasket();
        }

        public IDictionary<int, BasketItem> GetBasket()
        {
            return _basket.GetBasket();
        }

        public int GetBasketCount()
        {
            return _basket.GetBasketCount();
        }

        public decimal GetTotalPrice()
        {
            return _basket.GetTotalPrice();
        }

        public void MinusQuantity(int id)
        {
            var res = _basket.GetItemById(id).RemoveItem();
            if(res == 0)
            {
                _basket.RemoveFromBasket(id);
            }
            _basket.SaveToSession();
        }

        public void RemoveFromBasket(int id)
        {
            _basket.RemoveFromBasket(id);
        }

        public void SetBasket(Dictionary<int, BasketItem> basket)
        {
            _basket.SetBasket(basket);
        }
    }
}
