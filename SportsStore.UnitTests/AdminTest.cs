using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
               new Product { ProductID = 1,Name = "P1" },
               new Product { ProductID = 2,Name = "P2" },
               new Product { ProductID = 3,Name = "P3" }
            });

            AdminController target = new AdminController(mock.Object);

            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0].Name, "P1");
            Assert.AreEqual(result[1].Name, "P2");
            Assert.AreEqual(result[2].Name, "P3");
        }

        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
               new Product { ProductID = 1,Name = "P1" },
               new Product { ProductID = 2,Name = "P2" },
               new Product { ProductID = 3,Name = "P3" }
            });

            AdminController target = new AdminController(mock.Object);

            Product result = (Product)target.Edit(2).ViewData.Model;

            Assert.AreEqual(2, result.ProductID);
        }

        public void Canot_Edit_Nonexistent_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
               new Product { ProductID = 1,Name = "P1" },
               new Product { ProductID = 2,Name = "P2" },
               new Product { ProductID = 3,Name = "P3" }
            });

            AdminController target = new AdminController(mock.Object);

            Product result = (Product)target.Edit(4).ViewData.Model;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController target = new AdminController(mock.Object);

            var product = new Product { Name = "P1" };
            var result = target.Edit(product);

            mock.Verify(m => m.SaveProduct(product));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController target = new AdminController(mock.Object);

            var product = new Product { Name = "P1" };
            target.ModelState.AddModelError("error", "error");
            var result = target.Edit(product);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            var prod = new Product { ProductID = 2, Name = "P2" };
            mock.Setup(m => m.Products).Returns(new Product[]
            {
               new Product { ProductID = 1,Name = "P1" },
               prod,
               new Product { ProductID = 3,Name = "P3" }
            });

            AdminController target = new AdminController(mock.Object);

            var result = target.Delete(prod.ProductID);

            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
