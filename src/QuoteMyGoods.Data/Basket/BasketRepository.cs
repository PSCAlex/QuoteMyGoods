using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics;
using QuoteMyGoods.Data.Products;
using Newtonsoft.Json;
using QuoteMyGoods.Data.Common;

namespace QuoteMyGoods.Data.Basket
{
    public interface IBasketRepository
    {
        void ClearBasket();

        decimal GetTotalPrice();

        void AddToBasket(ProductDM product);

        void RemoveFromBasket(int id);

        int GetBasketCount();

        BasketDM GetBasket();

        BasketItem GetItemById(int id);

        void SaveToSession();

        void SetBasket(BasketDM basket);
    }

    public class BasketRepository : IBasketRepository
    {
        private IHttpContextAccessor _context;
        private UserManager<IdentityUser> _userManager;
        private BasketDM _Basket;

        public int BasketCount { get; private set; }
        public string UserId { get; private set; }

        public BasketRepository()
        {

        }

        public BasketRepository(IHttpContextAccessor context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            UserId = _userManager.GetUserId(_context.HttpContext.User);

            try
            {
                var basket = _context.HttpContext.Session.GetObjectFromJson<BasketDM>(UserId);
                if (basket == null)
                {
                    _Basket = new BasketDM();
                    BasketCount = _Basket.BasketCount;
                    _context.HttpContext.Session.SetObjectAsJson(UserId, this);
                }
                else
                {
                    _Basket = basket;
                    BasketCount = basket.BasketCount;
                }
            }
            catch (Exception ex)
            {
                _Basket = new BasketDM();
                BasketCount = 0;
                Debug.WriteLine("context was null", ex);
            }

        }

        public void ClearBasket()
        {
            _Basket = new BasketDM();
            BasketCount = 0;
            _context.HttpContext.Session.SetObjectAsJson(UserId, _Basket);
            SaveToSession();
        }

        public decimal GetTotalPrice()
        {
            return _Basket.Basket.Sum(b => b.Value.GetTotalPrice());
        }

        public void AddToBasket(ProductDM product)
        {
            if (!_Basket.Basket.ContainsKey(product.Id))
            {
                var b = new BasketItem();
                b.AddProduct(product);
                _Basket.Basket.Add(product.Id, b);
            }
            else
            {
                var basketItem = _Basket.Basket.FirstOrDefault(b => b.Key == product.Id);
                basketItem.Value.AddItem();
            }
            SaveToSession();
        }

        public void RemoveFromBasket(int id)
        {
            _Basket.Basket.Remove(id);
            SaveToSession();
        }

        public int GetBasketCount()
        {
            return _Basket.Basket.Sum(b => b.Value.GetQuantity());
        }

        public BasketDM GetBasket()
        {
            return _Basket;
        }

        public BasketItem GetItemById(int id)
        {
            return _Basket.Basket.FirstOrDefault(b => b.Key == id).Value;
        }

        public void SaveToSession()
        {
            var s = JsonConvert.SerializeObject(this);
            _context.HttpContext.Session.SetObjectAsJson(UserId, this);
        }

        public void SetBasket(BasketDM basket)
        {
            _Basket = basket ?? new BasketDM();
            BasketCount = basket.Basket.Sum(b => b.Value.GetQuantity());
            SaveToSession();
        }
    }
}
