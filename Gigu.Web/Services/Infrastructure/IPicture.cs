using Gigu.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Services.Infrastructure
{
    public interface IPicture
    {
        Picture GetById(int id);

        void Insert(Picture pict);

        void Update(Picture pict);

        void Delete(int id);

        void Save();
    }
}
