using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using QuoteMyGoods.Common;
using QuoteMyGoods.Models;
using QuoteMyGoods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuoteMyGoods.Controllers.Basket
{
    [Authorize]
    public class BasketController:Controller
    {
        IBasketService _basketService;
        IQMGRepository _repository;
        ILoggingService _logger;
        IBlobbingService _blobber;

        public BasketController(IBasketService basketService, IQMGRepository repository,ILoggingService logger, IBlobbingService blobber)
        {
            _basketService = basketService;
            _repository = repository;
            _logger = logger;
            _blobber = blobber;
        }
        
        public IActionResult Basket()
        {
            var basket = _basketService.GetBasket();
            ViewBag.TotalPrice = _basketService.GetTotalPrice();
            return View(basket);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(int? id, string orderbyList, string categoryList, string pageNumber, string itemsPerPage)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var product = await _repository.GetProductById(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                _basketService.AddToBasket(product);
                _logger.Log(HttpContext.User.GetUserId(), "AddToBasket");
                return RedirectToAction("Products", "Products", new { orderbyList = orderbyList, categoryList = categoryList, pageNumber = pageNumber, itemsPerPage = itemsPerPage });
            }
        }

        [HttpPost]
        public IActionResult RemoveFromBasket(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                _basketService.RemoveFromBasket((int)id);
                _logger.Log(HttpContext.User.GetUserId(), "RemoveFromBakset");
                return RedirectToAction("Basket");
            }
        }

        [HttpPost]
        public IActionResult AddQuantity(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            else
            {
                _basketService.AddQuantity((int)id);
                _logger.Log(HttpContext.User.GetUserId(), "AddQuantity");
                return RedirectToAction("Basket");
            }
        }

        [HttpPost]
        public IActionResult MinusQuantity(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            else
            {
                _basketService.MinusQuantity((int)id);
                _logger.Log(HttpContext.User.GetUserId(), "MinusQuantity");
                return RedirectToAction("Basket");
            }
        }

        [HttpPost]
        public IActionResult ClearBasket()
        {
            _basketService.ClearBasket();
            _logger.Log(HttpContext.User.GetUserId(), "ClearBasket");
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public IActionResult SaveBasket()
        {
            var reference = HttpContext.User.GetUserId() + "basketBlob";
            _blobber.UploadBlob(reference, _basketService.GetBasket());
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public IActionResult LoadBasket()
        {
            var reference = HttpContext.User.GetUserId() + "basketBlob";
            _basketService.SetBasket(_blobber.GetBlob<Dictionary<int, BasketItem>>(reference));
            return RedirectToAction("Basket");
        }
    }
}
