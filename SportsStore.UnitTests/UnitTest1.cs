using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="P1" },
                new Product {ProductID=1,Name="P2" },
                new Product {ProductID=1,Name="P3" },
                new Product {ProductID=1,Name="P4" },
                new Product {ProductID=1,Name="P5" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a><a class=""btn btn-default btn-primary selected"" href=""Strona2"">2</a><a class=""btn btn-default"" href=""Strona3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="P1" },
                new Product {ProductID=1,Name="P2" },
                new Product {ProductID=1,Name="P3" },
                new Product {ProductID=1,Name="P4" },
                new Product {ProductID=1,Name="P5" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="P1",Category = "Cat1" },
                new Product {ProductID=1,Name="P2" ,Category = "Cat2"},
                new Product {ProductID=1,Name="P3" ,Category = "Cat1"},
                new Product {ProductID=1,Name="P4" ,Category = "Cat2"},
                new Product {ProductID=1,Name="P5" ,Category = "Cat4"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="P1",Category = "B" },
                new Product {ProductID=2,Name="P2" ,Category = "A"},
                new Product {ProductID=3,Name="P3" ,Category = "B"},
                new Product {ProductID=4,Name="P4" ,Category = "A"},
                new Product {ProductID=5,Name="P5" ,Category = "C"}
            });

            NavController controller = new NavController(mock.Object);

            var result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            Assert.IsTrue(result[0] == "A");
            Assert.IsTrue(result[1] == "B");
            Assert.IsTrue(result[2] == "C");
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="P1",Category = "Cat1" },
                new Product {ProductID=2,Name="P2" ,Category = "Cat2"},
                new Product {ProductID=3,Name="P3" ,Category = "Cat1"},
                new Product {ProductID=4,Name="P4" ,Category = "Cat2"},
                new Product {ProductID=5,Name="P5" ,Category = "Cat4"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result1 = ((ProductsListViewModel)controller.List("Cat1", 1).Model).PagingInfo.TotalItems;
            var result2 = ((ProductsListViewModel)controller.List("Cat2", 1).Model).PagingInfo.TotalItems;
            var result3 = ((ProductsListViewModel)controller.List("Cat4", 1).Model).PagingInfo.TotalItems;

            Assert.AreEqual(result1,2);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3, 1);
        }
    }
}
