using QuoteMyGoods.Data.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Data.Basket
{
    public class BasketItem
    {
        public ProductDM Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public BasketItem() { }

        public void AddProduct(ProductDM product)
        {
            Product = product;
            Quantity = 1;
            TotalPrice = product.Price;
        }

        public void AddItem()
        {
            TotalPrice += Product.Price;
            Quantity++;
        }

        public int RemoveItem()
        {
            TotalPrice -= Product.Price;
            Quantity--;
            return Quantity;
        }

        public decimal GetTotalPrice()
        {
            return TotalPrice;
        }

        public string GetProductName()
        {
            return Product.Name;
        }

        public ProductDM GetProduct()
        {
            return Product;
        }

        public int GetQuantity()
        {
            return Quantity;
        }
    }
}
