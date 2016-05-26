using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Business.Basket;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{
    public class BasketCountViewComponent:ViewComponent
    {
        private IBasketService _basketService;

        public BasketCountViewComponent(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Count = _basketService.GetBasketCount();
            return View();
        }
    }
}
