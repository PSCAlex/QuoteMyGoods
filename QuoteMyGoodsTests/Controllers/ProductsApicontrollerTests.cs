using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Moq;
using Xunit;
using QuoteMyGoods.Models;
using QuoteMyGoods.Controllers.Api;
using AutoMapper;

namespace QuoteMyGoodsTests.Controllers
{
    public class ProductsApicontrollerTests
    {
        [Fact]
        public void GetReturnsJsonResultWithProducts()
        {
            var mockRepo = new Mock<IQMGRepository>();
            mockRepo.Setup(r => r.GetAllProducts()).Returns(GetTestProducts());
            var controller = new ProductsApiController(mockRepo.Object);

            Assert.IsType<JsonResult>(controller.Get());
        }

        [Fact]
        public void GetDotsReturnsJsonResult()
        {
            var mockRepo = new Mock<IQMGRepository>();
            var controller = new ProductsApiController(mockRepo.Object);

            Assert.IsType<JsonResult>(controller.GetDots());           
        }

        private async Task<IEnumerable<Product>> GetTestProducts()
        {
            var products = new List<Product>();
            products.Add(new Product()
            {
                Name = "Mahogany Plank",
                Category = "Timber",
                Description = "An mahogany plank of timber",
                Price = 8.0m,
                ImgUrl = "http://www.countrymouldings.com/images/butcher-block-countertops/prefinished-mahogany-plank-countertop-m.jpg"
            });
            products.Add(new Product()
            {
                Name = "Nails",
                Category = "General",
                Description = "Some nails",
                Price = 9.0m,
                ImgUrl = "http://s7g3.scene7.com/is/image/ae235/cat840028?$catImageSmall$"
            });
            return await Task.FromResult(products.AsEnumerable());
        }
    }
}
