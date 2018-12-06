using Gigu.Web.DataContext;
using Gigu.Web.Models;
using Gigu.Web.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Services.Repository
{
    public class CategoryRepository : ICategory
    {
        private readonly GiguContext _db;
        public CategoryRepository(GiguContext db)
        {
            _db = db;
        }
        public int Count() => _db.Category.Count();

        public void Delete(int id)
        {
            var category = GetById(id);
            if(category!=null)
            {
                _db.Category.Remove(category);
            }
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Category.Include(c => c.Products).Select(c => c);
        }

        public Category GetById(int id)
        {
            return _db.Category.FirstOrDefault(c => c.CategoryId == id);
        }

        public void Insert(Category cat)
        {
            _db.Category.Add(cat);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Category cat)
        {
            _db.Category.Update(cat);
        }
    }
}
