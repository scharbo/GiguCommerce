using Gigu.Web.DataContext;
using Gigu.Web.Models;
using Gigu.Web.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Services.Repository
{
    public class PictureRepository:IPicture
    {
        private readonly GiguContext _db;

        public PictureRepository(GiguContext db)
        {
            _db = db;
        }

        public void Delete(int id)
        {
            var picture = GetById(id);
            if (picture != null)
            {
                _db.Picture.Remove(picture);
            }
        }

        public Picture GetById(int id)
        {
            return _db.Picture.FirstOrDefault(p => p.PictureId == id);
        }

        public void Insert(Picture pict)
        {
            _db.Picture.Add(pict);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Picture pict)
        {
            _db.Picture.Update(pict);
        }
    }
}
