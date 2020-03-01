using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyeShop.Core.Models;
using MyeShop.DataAccess.InMemory;

namespace MyeShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        public ProductCategoryRepository _context;

        public ProductCategoryManagerController()
        {
            _context = new ProductCategoryRepository();
        }
        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            List<ProductCategory> products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
                return View(productCategory);

            _context.Insert(productCategory);
            _context.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = _context.Find(Id);

            if (productCategory == null)
                return HttpNotFound();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            ProductCategory productCategoryToUpdate = _context.Find(Id);

            if (productCategoryToUpdate == null)
                return HttpNotFound();

            if (!ModelState.IsValid)
                return View(productCategoryToUpdate);

            productCategoryToUpdate.Category = productCategory.Category;
            _context.Commit();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            ProductCategory productCategoryToDelete = _context.Find(Id);

            if (productCategoryToDelete == null)
                return HttpNotFound();

            _context.Delete(Id);
            _context.Commit();
            return RedirectToAction("Index");
        }
    }
}