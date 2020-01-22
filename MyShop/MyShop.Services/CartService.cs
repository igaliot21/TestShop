using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class CartService
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
        public void AddtoCart(HttpContextBase httpContext, string productID){
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
    }
}
