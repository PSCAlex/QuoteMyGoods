using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Areas.Products.Models;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{

    public class ProductDeleteViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ProductVM product)
        {
            return View(product);
        }
    }
}
