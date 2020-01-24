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
        IRepository<Customer> customers;
        ICartService cartService;
        IOrderService orderService;
        public CartController(ICartService CartService, IOrderService OrderService, IRepository<Customer> Customers)
        {
            this.cartService = CartService;
            this.orderService = OrderService;
            this.customers = Customers;
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
        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);
            if (customer != null)
            {
                Order order = new Order()
                {
                    City = customer.City,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    State = customer.State,
                    Street = customer.Street,
                    ZipCode = customer.ZipCode
                };
                return View(order);
            }
            else {
                return RedirectToAction("Error");
            }
         
        }
        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order) 
        {

            var cartItems = cartService.GetCartItems(this.HttpContext);
            order.OrderStatus = "Order created";
            order.Email = User.Identity.Name;

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