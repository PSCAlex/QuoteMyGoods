using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuoteMyGoods.Data.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Business.Products
{
    public interface IProductService
    {
        void AddProduct(ProductBM newProduct);
        int CurrentProductCount();
        void DeleteProduct(int id);
        Task<IEnumerable<ProductBM>> GetAllProducts();
        Task<SelectList> GetCategories();
        Task<ProductBM> GetProductById(int? id);
        Task<int> GetProductCount();
        Task<IEnumerable<ProductBM>> GetProductsByCategory(string categoryName);
        void UpdateProduct(ProductBM product);
    }

    public class ProductService:IProductService
    {
        private IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public void AddProduct(ProductBM newProduct)
        {
            _repo.AddProduct(Mapper.Map<ProductDM>(newProduct));
        }

        public int CurrentProductCount()
        {
            return _repo.CurrentProductCount();
        }

        public void DeleteProduct(int id)
        {
            _repo.DeleteProduct(id);
        }

        public async Task<IEnumerable<ProductBM>> GetAllProducts()
        {
            return Mapper.Map<IEnumerable<ProductBM>>(await _repo.GetAllProducts());
        }

        public async Task<SelectList> GetCategories()
        {
            return await _repo.GetCategories();
        }

        public async Task<ProductBM> GetProductById(int? id)
        {
            return Mapper.Map<ProductBM>(await _repo.GetProductById(id));
        }

        public async Task<int> GetProductCount()
        {
            return await _repo.GetProductCount();
        }

        public async Task<IEnumerable<ProductBM>> GetProductsByCategory(string categoryName)
        {
            return Mapper.Map<IEnumerable<ProductBM>>(await _repo.GetProductsByCategory(categoryName));
        }

        public void UpdateProduct(ProductBM product)
        {
            _repo.UpdateProduct(Mapper.Map<ProductDM>(product));
        }
    }
}
