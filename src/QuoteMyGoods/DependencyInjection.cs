using Microsoft.Extensions.DependencyInjection;
using QuoteMyGoods.Business.Basket;
using QuoteMyGoods.Business.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods
{
    public class DependencyInjection
    {
        public static void Configuration(IServiceCollection services)
        {
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
