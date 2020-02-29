using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyeShop.Core.Models;
using MyeShop.DataAccess.InMemory;

namespace MyeShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        public ProductRepository _context;

        public ProductManagerController()
        {
            _context = new ProductRepository();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            _context.Insert(product);
            _context.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            Product product = _context.Find(Id);

            if (product == null)
                return HttpNotFound();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToUpdate = _context.Find(Id);

            if (productToUpdate == null)
                return HttpNotFound();

            if (!ModelState.IsValid)
                return View(product);

            productToUpdate.Category = product.Category;
            productToUpdate.Description = product.Description;
            productToUpdate.Image = product.Image;
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

            _context.Delete(Id);
            _context.Commit();
            return RedirectToAction("Index");
        }
    }
}