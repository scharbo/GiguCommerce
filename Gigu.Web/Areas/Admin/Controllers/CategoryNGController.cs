using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigu.Web.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Gigu.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class CategoryNGController : Controller
    {
        private readonly ICategory _categoryRepository;



        public CategoryNGController(ICategory categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Models.Category Get(int id)
        {
            var category = _categoryRepository.GetById(id);
            return category;
        }

        public IEnumerable<Models.Category> GetAll()
        {
            var categories = _categoryRepository.GetAll().ToList();
            return categories;
        }
 
        [HttpPost]
        public IActionResult Create([FromBody] Models.Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _categoryRepository.Insert(category);
            _categoryRepository.Save();
            return Ok(category);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Models.Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _categoryRepository.Update(category);
            _categoryRepository.Save();
            return NoContent();
        }

        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            _categoryRepository.Delete(id);
            _categoryRepository.Save();
            return Ok(category);
        }
        

    }
}