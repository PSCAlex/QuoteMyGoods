using AutoMapper;
using QuoteMyGoods.Business.Products;
using QuoteMyGoods.Data.Basket;
using QuoteMyGoods.Data.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Business.Basket
{
    public interface IBasketService
    {
        void AddQuantity(int id);

        void AddToBasket(ProductBM product);

        void ClearBasket();

        BasketBM GetBasket();

        int GetBasketCount();

        decimal GetTotalPrice();

        void MinusQuantity(int id);

        void RemoveFromBasket(int id);

        void SetBasket(BasketBM basket);
    }

    public class BasketService : IBasketService
    {
        private IBasketRepository _repo;

        public BasketService(IBasketRepository repo)
        {
            _repo = repo;
        }

        public void AddQuantity(int id)
        {
            _repo.GetItemById(id).AddItem();
            _repo.SaveToSession();
        }

        public void AddToBasket(ProductBM product)
        {
            _repo.AddToBasket(Mapper.Map<ProductDM>(product));
        }

        public void ClearBasket()
        {
            _repo.ClearBasket();
        }

        public BasketBM GetBasket()
        {
            return Mapper.Map<BasketBM>(_repo.GetBasket());
        }

        public int GetBasketCount()
        {
            return _repo.GetBasketCount();
        }

        public decimal GetTotalPrice()
        {
            return _repo.GetTotalPrice();
        }

        public void MinusQuantity(int id)
        {
            var res = _repo.GetItemById(id).RemoveItem();
            if (res == 0)
            {
                _repo.RemoveFromBasket(id);
            }
            _repo.SaveToSession();
        }

        public void RemoveFromBasket(int id)
        {
            _repo.RemoveFromBasket(id);
        }

        public void SetBasket(BasketBM basket)
        {
            _repo.SetBasket(Mapper.Map<BasketDM>(basket));
        }
    }
}
