using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using QuoteMyGoods.Models;
using QuoteMyGoods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace QuoteMyGoods.Controllers.Products
{
    [Authorize(Roles = "Pleb,Administrator")]
    public class ProductsController:Controller
    {
        private IQMGRepository _repository;
        private IBasketService _basketService;
        private ILoggingService _logger;
        private IRedisService _redisService;

        public ProductsController(IQMGRepository repository, IBasketService basketService,ILoggingService logger,IRedisService redisService)
        {
            _repository = repository;
            _basketService = basketService;
            _logger = logger;
            _redisService = redisService;
        }

        public async Task<IActionResult> Products(string orderbyList, string categoryList, int? itemsPerPage, int? pageNumber)
        {
            _logger.Log(HttpContext.User.GetUserId(), "GetProducts");
            IEnumerable<Product> products = await _repository.GetProductsByCategory(categoryList);

            if (string.IsNullOrWhiteSpace(itemsPerPage.ToString()))
            {
                itemsPerPage = HttpContext.Session.GetInt32("itemsPerPage") ?? 6;
            }

            if (string.IsNullOrWhiteSpace(pageNumber.ToString()))
            {
                pageNumber = HttpContext.Session.GetInt32("pageNumber") ?? 1;
                if (pageNumber > (products.Count() / itemsPerPage))
                {
                    pageNumber = 1;
                }
            }

            AddInfoToSession((int)itemsPerPage, (int)pageNumber);

            ViewData["categoryList"] = await _repository.GetCategories();

            ViewData["orderbyList"] = new SelectList(new List<string>() { "Name", "Price low-to-high", "Price high-to-low", "Category" });

            if (!string.IsNullOrEmpty(orderbyList))
            {
                products = _OrderBy(products, orderbyList);
            }

            var pagedProducts = products.Skip(((int)pageNumber - 1) * (int)itemsPerPage).Take((int)itemsPerPage);

            //_redisService.SetObject("Products", pagedProducts);
            //var t = _redisService.GetObject<IEnumerable<Product>>("Products");

            return View(pagedProducts);
        }

        private void AddInfoToSession(int itemsPerPage, int pageNumber)
        {
            _logger.Log(HttpContext.User.GetUserId(), "AddInfoToSession");
            HttpContext.Session.SetInt32("itemsPerPage", itemsPerPage);
            HttpContext.Session.SetInt32("pageNumber", pageNumber);
        }

        private IEnumerable<Product> _OrderBy(IEnumerable<Product> products,string _string)
        {
            switch (_string)
            {
                case "Name":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "Price low-to-high":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "Price high-to-low":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "Category":
                    products = products.OrderBy(p => p.Category);
                    break;
                default:
                    break;
            }
            return products;
        }

        public async Task<IActionResult> Details(int? id)
        {
            _logger.Log(HttpContext.User.GetUserId(), "GetProductDetails");
            if (id == null)
            {
                return HttpNotFound();
            }

            var product = await _repository.GetProductById(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            _logger.Log(HttpContext.User.GetUserId(), "DeleteProducts");
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = await _repository.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.Log(HttpContext.User.GetUserId(), "DeleteProductsConfirmed");
            _repository.DeleteProduct(id);
            _repository.SaveAll();
            return RedirectToAction("Products");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _logger.Log(HttpContext.User.GetUserId(), "CreateProduct");
            if (ModelState.IsValid)
            {
                _repository.AddProduct(product);
                _repository.SaveAll();
                return RedirectToAction("Products");
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            _logger.Log(HttpContext.User.GetUserId(), "GetEditProduct");
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = await _repository.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _logger.Log(HttpContext.User.GetUserId(), "EditProduct");
            if (ModelState.IsValid)
            {
                _repository.UpdateProduct(product);
                _repository.SaveAll();
                return RedirectToAction("Products");
            }
            return View(product);
        }
    }
}
