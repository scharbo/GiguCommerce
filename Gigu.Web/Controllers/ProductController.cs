﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigu.Web.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Gigu.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _productRepository;
        private readonly ICategory _categoryRepository;

        public ProductController(IProduct productRepository, ICategory categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var products = _productRepository.GetAll().ToList();
            return View(products);
        }

        public IActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
                // or  return RedirectToAction("Home", "Error");
            }
            return View(_productRepository.GetById(Convert.ToInt32(id)));
        }
    }
}