using Gigu.Web.DataContext;
using Gigu.Web.Models;
using Gigu.Web.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Services.Repository
{
    public class OrderLineRepository:IOrderLine
    {
        private readonly GiguContext _db;

        public OrderLineRepository(GiguContext db)
        {
            _db = db;
        }

        public int Count()
        {
            return _db.OrderLine.Count();
        }

        public IEnumerable<OrderLine> GetAll()
        {
            return _db.OrderLine.Select(o => o);
        }

        public OrderLine GetById(int id)
        {
            return _db.OrderLine.FirstOrDefault(o => o.OrderLineId == id);
        }

        public void Insert(OrderLine orderLine)
        {
            _db.OrderLine.Add(orderLine);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(OrderLine orderLine)
        {
            _db.OrderLine.Update(orderLine);
        }
    }
}
