﻿using Microsoft.AspNet.Mvc;
using QuoteMyGoods.Common;
using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{
    public class BasketItemViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(BasketItem basketItem)
        {
            return View(basketItem);
        }
    }
}