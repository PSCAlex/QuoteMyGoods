using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuoteMyGoods.Business.Products;
using QuoteMyGoods.Business.Basket;
using AutoMapper;
using QuoteMyGoods.Areas.Products.Models;

namespace QuoteMyGoods.Areas.Products.Controllers
{
    [Area("Products")]
    [Authorize(Roles = "Pleb,Administrator")]
    public class ProductsController:Controller
    {
        private IProductService _productService;
        private IBasketService _basketService;

        public ProductsController(IProductService productService, IBasketService basketService)
        {
            _productService = productService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Products(string orderbyList, string categoryList, int? itemsPerPage, int? pageNumber)
        {
            IEnumerable<ProductVM> products = Mapper.Map<IEnumerable<ProductVM>>(await _productService.GetProductsByCategory(categoryList));

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

            ViewData["categoryList"] = await _productService.GetCategories();

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
            HttpContext.Session.SetInt32("itemsPerPage", itemsPerPage);
            HttpContext.Session.SetInt32("pageNumber", pageNumber);
        }

        private IEnumerable<ProductVM> _OrderBy(IEnumerable<ProductVM> products,string _string)
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
            if (id == null)
            {
                return NotFound();
            }

            var product = Mapper.Map<ProductVM>(await _productService.GetProductById(id));

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = Mapper.Map<ProductVM>(await _productService.GetProductById(id));
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Products");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(Mapper.Map<ProductBM>(product));
                return RedirectToAction("Products");
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = Mapper.Map<ProductVM>(await _productService.GetProductById(id));
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(Mapper.Map<ProductBM>(product));
                return RedirectToAction("Products");
            }
            return View(product);
        }
    }
}
