using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyeShop.Core.Contracts;
using MyeShop.Core.Models;
using MyeShop.Core.ViewModels;

namespace MyeShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IRepository<Product> _context;
        public IRepository<ProductCategory> productCategories;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            _context = productContext;
            productCategories = productCategoryContext;
        }

        public ActionResult Index(string Category = null)
        {

            List<Product> products;
            List<ProductCategory> categories = productCategories.Collection().ToList();

            if (Category == null)
            {
                products = _context.Collection().ToList();
            }
            else
            {
                products = _context.Collection().Where(p => p.Category == Category).ToList();
            }
            ProductListViewModel viewModel = new ProductListViewModel();

            viewModel.Products = products;
            viewModel.ProductCategories = categories;

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(string id)
        {
            Product product = _context.Find(id);
            if (product == null)
                return HttpNotFound();
            return View(product);
        }
    }
}