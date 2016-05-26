using AutoMapper;
using QuoteMyGoods.Business.Basket;
using QuoteMyGoods.Business.Products;
using QuoteMyGoods.Data.Basket;
using QuoteMyGoods.Data.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Business
{
    public static class ModelMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<BasketDM, BasketBM>();
            Mapper.CreateMap<BasketBM, BasketDM>();
            Mapper.CreateMap<ProductDM, ProductBM>();
            Mapper.CreateMap<ProductBM, ProductDM>();
        }
    }
}
