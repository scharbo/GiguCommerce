using Gigu.Web.DataContext;
using Gigu.Web.Models;
using Gigu.Web.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Services.Repository
{
    public class SubCategoryRepository : ISubCategory
    {
        private readonly GiguContext _db;
        public SubCategoryRepository(GiguContext db)
        {
            _db = db;
        }
        public int Count() => _db.SubCategory.Count();

        public void Delete(int id)
        {
            var subCategory = GetById(id);
            if(subCategory!=null)
            {
                _db.SubCategory.Remove(subCategory);
            }
        }

        public IEnumerable<SubCategory> GetAll()
        {
            return _db.SubCategory.Select(c => c);
        }

        public SubCategory GetById(int id)
        {
            return _db.SubCategory.FirstOrDefault(c => c.SubCategoryId == id);
        }

        public void Insert(SubCategory subCat)
        {
            _db.SubCategory.Add(subCat);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(SubCategory subCat)
        {
            _db.SubCategory.Update(subCat);
        }
    }
}
