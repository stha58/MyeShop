using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyeShop.Core.Contracts;
using MyeShop.Core.Models;
using MyeShop.Core.ViewModels;
using MyeShop.DataAccess.InMemory;

namespace MyeShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        public IRepository<Product> _context;
        private IRepository<ProductCategory> productCategories;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            _context = productContext;
            productCategories = productCategoryContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);

                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                _context.Insert(product);
                _context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = _context.Find(Id);

            if (product == null)
                return HttpNotFound();

            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = product;
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            Product productToUpdate = _context.Find(Id);

            if (productToUpdate == null)
                return HttpNotFound();

            if (!ModelState.IsValid)
                return View(product);

            if (file != null)
            {
                productToUpdate.Image = product.Id + Path.GetExtension(file.FileName);

                file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToUpdate.Image);
            }
            productToUpdate.Category = product.Category;
            productToUpdate.Description = product.Description;
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;

            _context.Commit();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            Product productToDelete = _context.Find(Id);

            if (productToDelete == null)
                return HttpNotFound();

            var currentImage = Server.MapPath("//Content//ProductImages//" + productToDelete.Image);

            _context.Delete(Id);
            if (System.IO.File.Exists(currentImage))
            {
                System.IO.File.Delete(currentImage);
            }
            _context.Commit();
            return RedirectToAction("Index");
        }
    }
}