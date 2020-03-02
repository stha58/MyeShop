using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyeShop.Core.Contracts;
using MyeShop.Core.Models;
using MyeShop.Core.ViewModels;
using MyeShop.Services;
using MyeShop.WebUI.Controllers;
using MyeShop.WebUI.Tests.Mocks;

namespace MyeShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItems()
        {
            //SetUp
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();

            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(products, baskets);

            var controller = new BasketController(basketService);
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller );

            //ACT
            //basketService.AddToBasket(httpContext, "1");
            controller.AddToBasket("1");

            Basket basket = baskets.Collection().FirstOrDefault();

            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }


        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();

            products.Insert(new Product(){Id = "1", Price = 10.00m});
            products.Insert(new Product(){Id = "2", Price = 20.00m});

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem(){ ProductId = "1", Quantity = 10});
            basket.BasketItems.Add(new BasketItem(){ ProductId = "2", Quantity = 4});
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);

            var controller = new BasketController(basketService);
            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new HttpCookie("eShopBasket") {Value = basket.Id});

            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel) result.ViewData.Model;

            Assert.AreEqual(14, basketSummary.BasketCount);
            Assert.AreEqual(180.00m, basketSummary.BasketTotal);
        }
    }
}
