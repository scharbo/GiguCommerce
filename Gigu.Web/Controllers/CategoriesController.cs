using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigu.Web.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Gigu.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class CategoriesController : Controller
    {
        private readonly ICategory _categoryRepository;

        public CategoriesController(ICategory categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var category = _categoryRepository.GetAll().ToList();
            return View(category);
        }
    }
}