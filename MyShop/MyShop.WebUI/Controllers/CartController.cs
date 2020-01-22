using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class CartController : Controller
    {
        ICartService cartService;
        public CartController(ICartService CartService){
            this.cartService = CartService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            var model = cartService.GetCartItems(this.HttpContext);
            return View(model);
        }
        public ActionResult AddToCart(string ProductID) {
            cartService.AddToCart(this.HttpContext, ProductID);
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromCart(string CartItemID){
            cartService.RemoveFromCart(this.HttpContext, CartItemID);
            return RedirectToAction("Index");
        }
        public PartialViewResult CartSummary(){
            var cartSummary = cartService.GetCartSummary(this.HttpContext);
            return PartialView(cartSummary);
        }
    }
}