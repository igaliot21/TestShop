using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> OrderContext) {
            this.orderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<CartItemViewModel> cartItems)
        {
            foreach (var item in cartItems){
                baseOrder.OrderItems.Add(new OrderItem() {
                    ProductID=item.ID,
                    Image=item.Image,
                    Price=item.Price,
                    ProductName=item.ProductName,
                    Quantity=item.Quantity
                });
            }
            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }
        public List<Order> GetOrderList() {
            return orderContext.Collection().ToList(); 
        }
        public Order GetOrder(string ID) {
            return orderContext.Find(ID);
        }
        public void UpdateOrder(Order updateOrder) {
            orderContext.Update(updateOrder);
            orderContext.Commit();
        }
    }
}
