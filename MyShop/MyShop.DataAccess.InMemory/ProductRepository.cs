using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository() {
            products = cache["products"] as List<Product>;
            if (products == null) {
                products = new List<Product>();
            }
        }
        public void Commit() {
            cache["products"] = products;
        }
        public void Insert(Product product) {
            products.Add(product);
        }
        public void Update(Product product) {
            Product productToUpdate = products.Find(p=>p.ID == product.ID);

            if (productToUpdate == null) {
                throw new Exception("Product not found");
            }
            else {
                productToUpdate = product;
            }
        }
        public Product Find(string ID) {
            Product product = products.Find(p => p.ID == ID);

            if (product == null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                return product;
            }
        }
        public IQueryable<Product> Collection() {
            return products.AsQueryable();
        }
        public void Delete(string ID) {
            Product productToDelete = products.Find(p => p.ID == ID);

            if (productToDelete == null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                products.Remove(productToDelete);
            }
        }
    }
}
