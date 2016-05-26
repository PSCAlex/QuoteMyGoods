using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuoteMyGoods.Data.Products
{
    public class ProductRepository
    {
        private ProductContext _context;
        private int _currentProductCount;
        private ILogger<ProductRepository> _logger;

        public ProductRepository(ProductContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
            _currentProductCount = 0;
            //_redisCache = new RedisCache(Startup.Configuration["RedisCache:ConnectionString"]);
        }

        public void AddProduct(ProductDM newProduct)
        {
            _context.Add(newProduct);
        }

        public int CurrentProductCount()
        {
            return _currentProductCount;
        }

        public void DeleteProduct(int id)
        {
            ProductDM product = _context.Products.FirstOrDefault(p => p.Id == id);
            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<ProductDM>> GetAllProducts()
        {
            /*
            var cachedProducts = await _redisCache.GetObject<List<Product>>("CachedProducts");
            if(cachedProducts == null || cachedProducts.Count() == 0)
            {*/
            var products = _context.Products.OrderBy(p => p.Name).ToList();
            _currentProductCount = products.Count;
            //_redisCache.SetObject("CachedProducts", products);
            return products;
            /*}else
            {
                _currentProductCount = cachedProducts.Count();
                return cachedProducts;
            }*/


        }

        public async Task<SelectList> GetCategories()
        {
            /*var cachedCategories = await _redisCache.GetObject<List<string>>("CachedCategories");
            if(cachedCategories == null || cachedCategories.Count() == 0)
            {*/
            var categoryQuery = from p in _context.Products
                                orderby p.Category
                                select p.Category;
            var categoryList = new List<string>();
            categoryList.AddRange(categoryQuery.Distinct());
            //_redisCache.SetObject("CachedCategories", categoryList);
            return new SelectList(categoryList);
            /*}else
            {
                return new SelectList(cachedCategories);
            }*/
        }

        public async Task<ProductDM> GetProductById(int? id)
        {
            /*var cachedProducts = await _redisCache.GetObject<List<Product>>("CachedProducts");
            if(cachedProducts == null || cachedProducts.Count() == 0)
            {*/
            return _context.Products.FirstOrDefault(p => p.Id == id);
            /*}else
            {
                return cachedProducts.FirstOrDefault(p => p.Id == id);
            }*/
        }

        public async Task<int> GetProductCount()
        {
            /*var cachedCount = await _redisCache.GetString("CachedCount");
            if (cachedCount == null)
            {*/
            //_redisCache.SetString("CachedCount", _context.Products.Count().ToString());
            return _context.Products.Count();
            /*}
            else
            {
                return int.Parse(cachedCount);
            }*/
        }

        public async Task<IEnumerable<ProductDM>> GetProductsByCategory(string categoryName)
        {
            /*var cachedProducts = await _redisCache.GetObject<List<Product>>("CachedProducts");
            if (cachedProducts == null || cachedProducts.Count() == 0)
            {*/
            if (string.IsNullOrEmpty(categoryName))
            {
                var products = _context.Products.OrderBy(p => p.Name).ToList();
                _currentProductCount = products.Count;
                //_redisCache.SetObject("CachedProducts", products);
                //_redisCache.SetString("CachedCount", _currentProductCount.ToString());
                return products;
            }
            else
            {
                var products = _context.Products.Where(p => p.Category == categoryName).ToList();
                _currentProductCount = products.Count;
                //_redisCache.SetObject("CachedProducts", products);
                //_redisCache.SetString("CachedCount", _currentProductCount.ToString());
                return products;
            }
            /*}
            else
            {
                if (string.IsNullOrEmpty(categoryName))
                {
                    _currentProductCount = cachedProducts.Count();
                    //_redisCache.SetString("CachedCount", _currentProductCount.ToString());
                    return cachedProducts.OrderBy(p => p.Name).ToList();
                }
                else
                {
                    _currentProductCount = cachedProducts.Count();
                    //_redisCache.SetString("CachedCount", _currentProductCount.ToString());
                    return cachedProducts.Where(p => p.Category == categoryName).ToList();
                }
            }*/
        }

        public bool SaveAll() => _context.SaveChanges() > 0;

        public void UpdateProduct(ProductDM product)
        {
            _context.Update(product);
        }
    }
}
