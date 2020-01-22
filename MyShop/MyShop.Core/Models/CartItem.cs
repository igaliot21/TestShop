using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class CartItem:BaseEntity
    {
        public string CartID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
