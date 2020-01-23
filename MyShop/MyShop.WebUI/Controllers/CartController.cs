using MyShop.Core.Contracts;
using MyShop.Core.Models;
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
        IOrderService orderService;
        public CartController(ICartService CartService, IOrderService OrderService)
        {
            this.cartService = CartService;
            this.orderService = OrderService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            var model = cartService.GetCartItems(this.HttpContext);
            return View(model);
        }
        public ActionResult AddToCart(string ProductID)
        {
            cartService.AddToCart(this.HttpContext, ProductID);
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromCart(string CartItemID)
        {
            cartService.RemoveFromCart(this.HttpContext, CartItemID);
            return RedirectToAction("Index");
        }
        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);
            return PartialView(cartSummary);
        }
        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            var cartItems = cartService.GetCartItems(this.HttpContext);
            order.OrderStatus = "Order created";

            //Process payment by 3rd party software

            order.OrderStatus = "Payment processed";
            orderService.CreateOrder(order, cartItems);
            cartService.clearCart(this.HttpContext);
            return RedirectToAction("Thankyou", new { OrderID = order.ID });
        }
        public ActionResult Thankyou(string OrderID)
        {
            ViewBag.OrderID = OrderID;
            return View();
        }
    }
}