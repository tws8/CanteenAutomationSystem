using Microsoft.VisualStudio.TestTools.UnitTesting;
using CanteenAutomationSystem.Areas.MyInfo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Moq;
using CanteenAutomationSystem_UnitTest;
using CanteenAutomationSystem.Models;

namespace CanteenAutomationSystem.Areas.MyInfo.Controllers.Tests
{
    [TestClass()]
    public class UnitTestControllerTests
    {
        [TestMethod()]
        public void RegisterCustomerUserTest()
        {
            var data = new List<Customer>
            {
                new Customer{ CustID="Test1", CustPW="Test1", CustName="Test1", CustMemberStatus="N", BalCredit=0 },
                new Customer{ CustID="Test2", CustPW="Test2", CustName="Test2", CustMemberStatus="N", BalCredit=0 },
                new Customer{ CustID="Test3", CustPW="Test3", CustName="Test3", CustMemberStatus="N", BalCredit=0 },

            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<CanteenContext>();
            mockContext.Setup(c => c.Customers).Returns(mockSet.Object);

            var service = new CanteenService(mockContext.Object);
            var customers = service.GetAllCustomers();

            Assert.AreEqual(3, customers.Count);

            Assert.AreEqual("Test1", customers[0].CustID);
            Assert.AreEqual("Test2", customers[1].CustID);
            Assert.AreEqual("Test3", customers[2].CustID);

            Assert.AreEqual("Test1", customers[0].CustName);
            Assert.AreEqual("Test2", customers[1].CustName);
            Assert.AreEqual("Test3", customers[2].CustName);

        }

        [TestMethod()]
        public void RegisterBusinessUserTest()
        {
            var data = new List<BizUser>
            {
                new BizUser{ BizUserID="Admin", BizUserPW="Admin", BizUserName="Admin", BizDeptID="A" },
                new BizUser{ BizUserID="Finance", BizUserPW="Finance", BizUserName="Finance", BizDeptID="F" },
                new BizUser{ BizUserID="Kitchen", BizUserPW="Kitchen", BizUserName="Kitchen", BizDeptID="K" },

            }.AsQueryable();

            var mockSet = new Mock<DbSet<BizUser>>();
            mockSet.As<IQueryable<BizUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<BizUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<BizUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<BizUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<CanteenContext>();
            mockContext.Setup(c => c.BizUsers).Returns(mockSet.Object);

            var service = new CanteenService(mockContext.Object);
            var bizUsers = service.GetAllBizUsers();

            Assert.AreEqual(3, bizUsers.Count);

            Assert.AreEqual("Admin", bizUsers[0].BizUserID);
            Assert.AreEqual("Finance", bizUsers[1].BizUserID);
            Assert.AreEqual("Kitchen", bizUsers[2].BizUserID);

            Assert.AreEqual("Admin", bizUsers[0].BizUserName);
            Assert.AreEqual("Finance", bizUsers[1].BizUserName);
            Assert.AreEqual("Kitchen", bizUsers[2].BizUserName);

        }
    }
}