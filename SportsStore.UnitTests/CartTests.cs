using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Items()
        {
            Product p1 = new Product { ProductID = 1 };
            Product p2 = new Product { ProductID = 2 };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            CartLine[] result = cart.Lines.ToArray();

            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ProductID = 1 };
            Product p2 = new Product { ProductID = 2 };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p2, 2);
            CartLine[] result = cart.Lines.ToArray();

            Assert.AreEqual(result[0].Quantity, 2);
            Assert.AreEqual(result[1].Quantity, 3);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ProductID = 1 };
            Product p2 = new Product { ProductID = 2 };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.RemoveLine(p1);
            CartLine[] result = cart.Lines.ToArray();

            Assert.AreEqual(result.Where(x => x.Product.ProductID == p1.ProductID).Count(), 0);
            Assert.AreEqual(result.Length, 1);

        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ProductID = 1 };
            Product p2 = new Product { ProductID = 2 };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.Clear();
            CartLine[] result = cart.Lines.ToArray();

            Assert.AreEqual(result.Length, 0);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product { ProductID = 1, Price = 1 };
            Product p2 = new Product { ProductID = 2, Price = 2 };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p2, 2);
            decimal total = cart.ComputeTotalValue();

            Assert.AreEqual(total, 8);
        }
    }
}
