using Gigu.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.Services.Infrastructure
{
    public interface IOrderLine
    {
        IEnumerable<OrderLine> GetAll();

        OrderLine GetById(int id);

        void Insert(OrderLine orderLine);

        void Update(OrderLine orderLine);

        int Count();

        void Save();
    }
}
