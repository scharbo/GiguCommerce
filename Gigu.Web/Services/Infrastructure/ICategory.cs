using Gigu.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Services.Infrastructure
{
    public interface ICategory
    {
        IEnumerable<Category> GetAll();

        Category GetById(int id);

        void Insert(Category cat);

        void Update(Category cat);

        void Delete(int id);

        int Count();

        void Save();
    }
}
