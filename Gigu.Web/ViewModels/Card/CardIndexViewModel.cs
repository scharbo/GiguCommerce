using Gigu.Web.Paypal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.ViewModels.Card
{
    public class CardIndexViewModel
    {
        public CardIndexViewModel()
        {
            CardProductVMList = new List<CardProductViewModel>();
            PayOrder = new PaypalOrder();
        }

        public List<CardProductViewModel> CardProductVMList { get; set; }
        public PaypalOrder PayOrder { get; set; }

        public decimal CardTotalPrice { get; set; }
    }
}
