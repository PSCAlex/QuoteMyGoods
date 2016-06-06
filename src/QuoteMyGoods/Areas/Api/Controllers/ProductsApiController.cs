using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Areas.Products.Models;
using QuoteMyGoods.Business.Products;
using System;
using System.Collections.Generic;
using System.Net;

namespace QuoteMyGoods.Areas.Api.Controllers
{
    [Area("Api")]
    //[Authorize]
    public class ProductsApiController:Controller
    {
        private IProductService _service;

        public ProductsApiController (IProductService service)
        {
            _service = service;
        }
        
        [HttpGet("api/products")]
        public JsonResult Get()
        {
            var products = Mapper.Map<IEnumerable<ProductVM>>(_service.GetAllProducts());

            return Json(products);
        }

        [HttpPost("api/products")]
        public JsonResult Post([FromBody]ProductVM product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _service.AddProduct(Mapper.Map<ProductBM>(product));

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(product);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState});
        }

        [HttpDelete("api/product")]
        public JsonResult Delete(int id)
        {
            _service.DeleteProduct(id);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { Message = "Deleted" });
        }

        [HttpGet("api/product")]
        public JsonResult GetById(int id)
        {
            var product = _service.GetProductById(id);
            return Json(Mapper.Map<ProductVM>(product));
        }

        [HttpPut("api/product")]
        public JsonResult Update([FromBody]ProductVM product)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateProduct(Mapper.Map<ProductBM>(product));
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(product);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed to update" });
        }

        [HttpPost("api/dots")]
        public JsonResult AddDots([FromBody]object dots)
        {
            if (!string.IsNullOrEmpty(dots.ToString()))
            {
                //TODO fix user obj
                //HttpContext.Session.SetString(HttpContext.User.GetJoinTheDots(), dots.ToString());
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Message = "Saved to session" });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "No body data" });
            }
        }

        [HttpGet("api/dots")]
        public JsonResult GetDots()
        {
            //TODO fix user obj
            var userJoinTheDotsKey = "";//HttpContext.User.GetJoinTheDots();
            var dots = HttpContext.Session.GetString(userJoinTheDotsKey);
            return Json(dots);
        }
    }
}
