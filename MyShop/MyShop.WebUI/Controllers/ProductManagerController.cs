﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this.context = productContext;
            this.productCategories = productCategoryContext;
        }
        // GET: ProductManager
        public ActionResult Index() 
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
        public ActionResult Create() {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            //Product product = new Product();
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else {
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult Edit(string ID)
        {
            Product product = context.Find(ID);
            if (product == null)
            {
                return HttpNotFound();
            }
            else {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();

                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();
                return View(viewModel);
                //return View(product);
            }

        }
        [HttpPost]
        public ActionResult Edit(Product product, string ID)
        {
            Product productToEdit = context.Find(ID);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                { 
                    return View(product);
                } 
                else
                {
                    productToEdit.Category      = product.Category;
                    productToEdit.Description   = product.Description;
                    productToEdit.Image         = product.Image;
                    productToEdit.Name          = product.Name;
                    productToEdit.Price         = product.Price;

                     context.Update(productToEdit);
                    context.Commit();
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Delete(string ID)
        {
            Product productToDelete = context.Find(ID);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);  
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string ID) {
            Product productToDelete = context.Find(ID);
            if (productToDelete == null){
                return HttpNotFound();
            }
            else {
                context.Delete(ID);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}
