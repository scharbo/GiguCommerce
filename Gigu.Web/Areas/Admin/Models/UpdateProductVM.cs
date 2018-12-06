using Gigu.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Areas.Admin.AdminVM
{
    public class UpdateProductVM
    {
        public Product Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
