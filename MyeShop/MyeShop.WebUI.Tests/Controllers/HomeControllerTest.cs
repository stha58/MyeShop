using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyeShop.Core.Contracts;
using MyeShop.Core.Models;
using MyeShop.Core.ViewModels;
using MyeShop.WebUI;
using MyeShop.WebUI.Controllers;
using MyeShop.WebUI.Tests.Mocks;

namespace MyeShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexPageDoesReturnProduct()
        {
            IRepository<Product> productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new Mocks.MockContext<ProductCategory>();

            productContext.Insert(new Product());

            // Arrange
            HomeController controller = new HomeController(productContext, productCategoryContext);

            // Act
            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel) result.ViewData.Model;

            // Assert
            Assert.AreEqual(1, viewModel.Products.Count());
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            //HomeController controller = new HomeController();

            //// Act
            //ViewResult result = controller.About() as ViewResult;

            //// Assert
            //Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            //HomeController controller = new HomeController();

            //// Act
            //ViewResult result = controller.Contact() as ViewResult;

            //// Assert
            //Assert.IsNotNull(result);
        }
    }
}
