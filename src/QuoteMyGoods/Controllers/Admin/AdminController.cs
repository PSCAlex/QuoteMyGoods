using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using QMGAzure;
using Microsoft.AspNet.Mvc.Rendering;
using QuoteMyGoods.Models;
using QuoteMyGoods.Services;
using QuoteMyGoods.Common;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace QuoteMyGoods.Controllers.Admin
{
    public class AdminController : Controller
    {
        private MyDocumentDB _db;
        private IQMGRepository _repository;
        private ITableService _tableService;

        public AdminController(IQMGRepository repo, ITableService tableService)
        {
            _db = new MyDocumentDB("alexpscdb");
            _repository = repo;
            _tableService = tableService;
        } 
        // GET: /<controller>/
        public IActionResult Index(string userId)
        {
            IEnumerable<LoggingDocument> logs =  _db.GetAllDocs("logging");

            ViewData["userId"] = new SelectList(logs.Select(l => l.UserId).Distinct());

            if (!string.IsNullOrWhiteSpace(userId))
            {
                logs = _db.GetDocsByUserId("logging", userId);
            }

            return View(logs);
        }

        public IActionResult Details(string id)
        {
            var log = _db.GetDocById("logging",id);
            return View(log);
        }

        public IActionResult Tables(string partition, string entityType)
        {
            switch (entityType){
                case "user":
                    IEnumerable<UserEntity> userTables = _tableService.GetTables(partition, new UserEntity().entityResolver);
                    return View(userTables);
                default:
                    IEnumerable<ProductEntity> productTables = _tableService.GetTables(partition, new ProductEntity().entityResolver);
                    return View(productTables);
            }
        }
    }
}
