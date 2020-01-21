 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository() {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null) {
                productCategories = new List<ProductCategory>();
            }
        }
        public void Commit(){
            cache["productCategories"] = productCategories;
        }
        public void Insert(ProductCategory productCategory){
            productCategories.Add(productCategory);
        }
        public void Update(ProductCategory productCategory){
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.ID == productCategory.ID);

            if (productCategoryToUpdate == null)
            {
                throw new Exception("Product category not found");
            }
            else
            {
                productCategoryToUpdate = productCategory;
            }
        }
        public ProductCategory Find(string ID)
        {
            ProductCategory productCategory = productCategories.Find(p => p.ID == ID);

            if (productCategory == null)
            {
                throw new Exception("Product category not found");
            }
            else
            {
                return productCategory;
            }
        }
        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }
        public void Delete(string ID)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.ID == ID);

            if (productCategoryToDelete == null)
            {
                throw new Exception("Product category not found");
            }
            else
            {
                productCategories.Remove(productCategoryToDelete);
            }
        }
    }
}
