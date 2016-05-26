using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Data.Basket;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{
    public class BasketItemViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BasketItem basketItem)
        {
            return View(basketItem);
        }
    }
}
