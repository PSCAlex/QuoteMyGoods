using AutoMapper;
using QuoteMyGoods.Areas.Basket.Models;
using QuoteMyGoods.Areas.Products.Models;
using QuoteMyGoods.Business.Basket;
using QuoteMyGoods.Business.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods
{
    public class ModelMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<ProductBM, ProductVM>();
            Mapper.CreateMap<ProductVM, ProductBM>();
            Mapper.CreateMap<BasketBM, BasketVM>();
            Mapper.CreateMap<BasketBM, BasketVM>();
        }
    }
}
