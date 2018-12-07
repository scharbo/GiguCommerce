using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigu.Web.Models;
using Gigu.Web.Services.Infrastructure;
using Gigu.Web.Areas.Admin.AdminVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gigu.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class ProductsController : Controller
    {
        private readonly IProduct _productRepository;
        private readonly ICategory _categoryRepository;
        private readonly UserManager<Customer> _userManager;
        private IHostingEnvironment _environment;

        public ProductsController(IProduct productRepository, ICategory categoryRepository, UserManager<Customer> userManager, IHostingEnvironment environment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _environment = environment;
    }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            var product = _productRepository.GetAll();

            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var createProduct = new CreateProductViewModel
            {
                Products = new Product(),
                Categories = _categoryRepository.GetAll().ToList()
            };
            return View(createProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateProductViewModel productVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //Create Image

            if (productVM.Products.ProductImage.Length > 0)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");

                using (var fileStream = new FileStream(Path.Combine(uploads, productVM.Products.ProductImage.FileName), FileMode.Create))
                {
                    productVM.Products.ProductImage.CopyTo(fileStream);
                }
                productVM.Products.ProductImagePath = productVM.Products.ProductImage.FileName.ToString();
            }



            _productRepository.Insert(productVM.Products);

            try
            {
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public IActionResult Update(int id)
        {
            var prod = new UpdateProductViewModel
            {
                Products = _productRepository.GetById(id),
                Categories = _categoryRepository.GetAll()
            };
            return View(prod);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, UpdateProductViewModel updateProductVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (updateProductVM.Products.ProductImage != null && updateProductVM.Products.ProductImage.Length > 0)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");

                using (var fileStream = new FileStream(Path.Combine(uploads, updateProductVM.Products.ProductImage.FileName), FileMode.Create))
                {
                    updateProductVM.Products.ProductImage.CopyTo(fileStream);
                }
                updateProductVM.Products.ProductImagePath = updateProductVM.Products.ProductImage.FileName.ToString();
            }

            _productRepository.Update(updateProductVM.Products);

            try
            {
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                return View(ex);
            }


            return RedirectToAction("Index");
        }
        
        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            var prod = new UpdateProductViewModel
            {
                Products = _productRepository.GetById(id),
                Categories = _categoryRepository.GetAll()
            };
            return View(prod);
        }

        //delete product on click delete
        [HttpPost("{id}")]
        public IActionResult Delete(int id, UpdateProductViewModel productVM)
        {
            var originalProd = _productRepository.GetById(productVM.Products.ProductId);
            _productRepository.Delete(productVM.Products.ProductId);
            _categoryRepository.Save();

            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            try
            {
                FileInfo f = new FileInfo(uploads + "/" + originalProd.ProductImagePath);
                f.Delete();
            }
            catch
            {
                //   throw ex;
            }

            if (TempData != null)
            {
                if (TempData.ContainsKey("result"))
                {
                    TempData["result"] = "Product Deleted";
                }
                else
                {
                    TempData.Add("result", "Product Deleted");
                }

            }

            return RedirectToAction("Index");
        }
        
    }
}
