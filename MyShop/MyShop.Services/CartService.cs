using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class CartService : ICartService
    {
        IRepository<Product> productContext;
        IRepository<Cart> cartContext;
        public const string CartSessionName = "eCommCart";
        public CartService(IRepository<Product> ProductContext,IRepository<Cart> CartContext) {
            this.cartContext = CartContext;
            this.productContext = ProductContext;
        }
        private Cart GetCart(HttpContextBase httpContext, bool createIfNull) {
            HttpCookie cookie = httpContext.Request.Cookies.Get(CartSessionName);

            Cart cart = new Cart();

            if (cookie != null){
                string cartID = cookie.Value;
                if (!string.IsNullOrEmpty(cartID)){
                    cart = cartContext.Find(cartID);
                }
                else{
                    if (createIfNull){
                        cart = CreateNewCart(httpContext);
                    }
                }
            }
            else {
                if (createIfNull){
                    cart = CreateNewCart(httpContext);
                }
            }
            return cart;
        }
        private Cart CreateNewCart(HttpContextBase httpContext) {
            Cart cart = new Cart();
            cartContext.Insert(cart);
            cartContext.Commit();

            HttpCookie cookie = new HttpCookie(CartSessionName);
            cookie.Value = cart.ID;
            cookie.Expires = DateTime.Now.AddDays(10);
            httpContext.Response.Cookies.Add(cookie);

            return cart;
        }
        public void AddToCart(HttpContextBase httpContext, string productID){
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(i=>i.ProductID==productID);

            if (item == null){
                item = new CartItem(){
                    CartID = cart.ID,
                    ProductID = productID,
                    Quantity = 1
                };
                cart.CartItems.Add(item);
            }
            else {
                item.Quantity++;
            }
            cartContext.Commit();
        }
        public void RemoveFromCart(HttpContextBase httpContext, string itemID) {
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(i => i.ID == itemID);

            if (item!=null) {
                cart.CartItems.Remove(item);
                cartContext.Commit();
            }
        }
        public List<CartItemViewModel> GetCartItems(HttpContextBase httpContext) {
            Cart cart = GetCart(httpContext, false);
            if (cart != null)
            {
                var results = (from c in cart.CartItems
                               join p in productContext.Collection()
                               on c.ProductID equals p.ID
                               select new CartItemViewModel()
                               {
                                   ID = c.ID,
                                   Quantity = c.Quantity,
                                   ProductName = p.Name,
                                   Price = p.Price,
                                   Image = p.Image
                               }).ToList();
                return results;
            }
            else {
                return new List<CartItemViewModel>();
            }
        }
        public CartSummaryViewModel GetCartSummary(HttpContextBase httpContext) {
            Cart cart = GetCart(httpContext, false);
            CartSummaryViewModel model = new CartSummaryViewModel(0, 0);
            if (cart != null) {
                int? cartCount = (from item in cart.CartItems
                                  select item.Quantity).Sum();
                decimal? cartTotal = (from item in cart.CartItems
                                         join p in productContext.Collection() 
                                         on item.ProductID equals p.ID
                                         select item.Quantity*p.Price).Sum();
                model.CartCount = cartCount ?? 0;
                model.CartTotal = cartTotal ?? decimal.Zero;
                return model;
            }
            else {
                return model;
            }
        }
    }
}
