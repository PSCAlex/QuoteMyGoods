using Microsoft.AspNet.Mvc;
using QuoteMyGoods.Models;

namespace QuoteMyGoods.ViewComponents
{

    public class ProductDeleteViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(Product product)
        {
            return View(product);
        }
    }
}
