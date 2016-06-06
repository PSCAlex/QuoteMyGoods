using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Areas.Basket.Models;
using QuoteMyGoods.Business.Basket;
using QuoteMyGoods.Business.Products;
using System.Threading.Tasks;

namespace QuoteMyGoods.Areas.Basket.Controllers
{
    [Area("Basket")]
    [Authorize]
    public class BasketController:Controller
    {
        private IBasketService _service;
        private ProductService _productService;

        public BasketController(IBasketService service, ProductService productService)
        {
            _service = service;
            _productService = productService;
        }
        
        public IActionResult Basket()
        {
            var basket = Mapper.Map<BasketVM>(_service.GetBasket());
            ViewBag.TotalPrice = _service.GetTotalPrice();
            return View(basket);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(int? id, string orderbyList, string categoryList, string pageNumber, string itemsPerPage)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _service.AddToBasket(product);
                return RedirectToAction("Products", "Products", new { orderbyList = orderbyList, categoryList = categoryList, pageNumber = pageNumber, itemsPerPage = itemsPerPage });
            }
        }

        [HttpPost]
        public IActionResult RemoveFromBasket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                _service.RemoveFromBasket((int)id);
                return RedirectToAction("Basket");
            }
        }

        [HttpPost]
        public IActionResult AddQuantity(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            else
            {
                _service.AddQuantity((int)id);
                return RedirectToAction("Basket");
            }
        }

        [HttpPost]
        public IActionResult MinusQuantity(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            else
            {
                _service.MinusQuantity((int)id);
                return RedirectToAction("Basket");
            }
        }

        [HttpPost]
        public IActionResult ClearBasket()
        {
            _service.ClearBasket();
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public IActionResult SaveBasket()
        {
            //TODO reintroduce when azure is working again
            //var reference = _userManager.GetUserId(HttpContext.User) + "basketBlob";
            //_blobber.UploadBlob(reference, _basketService.GetBasket());
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public IActionResult LoadBasket()
        {
            //TODO reintroduce when azure is working again
            //var reference = _userManager.GetUserId(HttpContext.User) + "basketBlob";
            //_basketService.SetBasket(_blobber.GetBlob<Dictionary<int, BasketItem>>(reference));
            return RedirectToAction("Basket");
        }
    }
}
