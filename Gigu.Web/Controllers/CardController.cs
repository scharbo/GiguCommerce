﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigu.Web.DataContext;
using Gigu.Web.Models;
using Gigu.Web.Paypal;
using Gigu.Web.Services.Infrastructure;
using Gigu.Web.ViewModels.Card;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gigu.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly GiguContext _db;
        private readonly UserManager<Customer> _userManager;
        private readonly ICartItem _cartItemRepository;
        private PaypalSettings _configSettings { get; set; }

        public CardController(GiguContext db, UserManager<Customer> userManager, ICartItem cartItemRepository, IOptions<PaypalSettings> settings)
        {
            _db = db;
            _userManager = userManager;
            _cartItemRepository = cartItemRepository;
            _configSettings = settings.Value;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var query = from p in _db.CartItem
                        where p.CustomerId == userId
                        group p by new { p.Product.ProductName, p.Product.UnitPrice } into g
                        select new
                        {
                            Name = g.Key.ProductName,
                            Count = g.Count(),
                            Price = g.Key.UnitPrice
                        };

            CardIndexViewModel cardVM = new CardIndexViewModel();
            foreach (var item in query)
            {
                CardProductViewModel cProd = new CardProductViewModel();

                cProd.ProductName = item.Name;
                cProd.Quantity = item.Count;
                cProd.Price = item.Price;
                cProd.ProductTotal = item.Price * item.Count;

                cardVM.CardProductVMList.Add(cProd);
            }

            cardVM.CardTotalPrice = cardVM.CardProductVMList.Sum(c => c.ProductTotal);
            return View(cardVM);
        }

        [HttpPost]
        public IActionResult Add([FromForm] int? id)
        {
            if (id != null)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                CartItem cartItem = new CartItem()
                {
                    CustomerId = userId,
                    AddedDate = DateTime.Now,
                    ProductId = Convert.ToInt32(id)
                };
                _db.CartItem.Add(cartItem);
                _db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "Product not found");
                return NotFound();
            }

            return RedirectToAction("Index", "Product");
        }
        
        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Return()
        {
            return Content(Request.QueryString.ToString());
        }

        public IActionResult Cancel()
        {
            return Content(Request.QueryString.ToString());
        }

        public IActionResult Pay(PaypalOrder po)
        {
            PaypalRedirect redirect = PaypalLogic.ExpressCheckout(po);

            //Session["token"] = redirect.Token;
            HttpContext.Session.SetString("token", redirect.Token);

            return new RedirectResult(redirect.Url);
        }

        
    }
}