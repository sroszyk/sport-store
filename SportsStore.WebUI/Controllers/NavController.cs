﻿using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IProductRepository repository;
        public NavController(IProductRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null, bool horizontalLayout = false)
        {
            ViewBag.SelectedCategory = category;
            var categories = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);
            string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";
            return PartialView(viewName, categories);
        }
    }
}