using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using AIC_Software.Controllers;
using AIC_Software.Models;
using AIC_Software.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AIC_Software.Tests
{
    [TestClass]
    public class AIC_Sw_HomeControllerTests
    {
        static Software[] staticCatalogData()
        {
            return new Software[] {
                new Software { Version = "1", Name = "app1" },
                new Software { Version = "2", Name = "app2" },
                new Software { Version = "2.1", Name = "app2.1" },
                new Software { Version = "2.3", Name = "app2.2" },
                new Software { Version = "2.0.1", Name = "app2.0.1" },
                new Software { Version = "2.0.2", Name = "app2.0.2" },
                new Software { Version = "2.0.3", Name = "app2.0.3" },
                new Software { Version = "3", Name = "app3" },
                new Software { Version = "10.0", Name = "app10.0" }
            };
        }

        [TestMethod]
        // test ability to mock the catalog interface and the SemVer filter
        public void Test_Home_Index_FilterString()
        {
            string filterString = "1.1";

            // The catalog interface represents the data access layer to 'future proof' the 
            // currently seeded data set with a database interface. Use Moq object for testing. 
            Mock<ISoftwareCatalog> mock = new Mock<ISoftwareCatalog>();
            mock.Setup(m => m.Products).Returns(staticCatalogData().AsQueryable<Software>());

            // instantiate the controller with the mock data object
            HomeController hc = new HomeController(mock.Object);

            // and inspect the data model from the viewresult from the Index() method
            // adding the string parameter ensures test against (overloaded) POST method only
            ViewResult view = (ViewResult)hc.Index(filterString);
            SoftwareCatalogViewModel catalog = (SoftwareCatalogViewModel)view.ViewData.Model;
            Assert.AreEqual(filterString, catalog.SoftwareVersionFilter);
        }

        [TestMethod]
        // return entire catalog
        public void Test_Home_Index_DisplayEntireCatalog()
        {
            string filterString = "0";

            Mock<ISoftwareCatalog> mock = new Mock<ISoftwareCatalog>();
            mock.Setup(m => m.Products).Returns(staticCatalogData().AsQueryable<Software>());

            HomeController hc = new HomeController(mock.Object);

            ViewResult view = (ViewResult)hc.Index(filterString);
            SoftwareCatalogViewModel catalog = (SoftwareCatalogViewModel)view.ViewData.Model;
            Assert.AreEqual(9, catalog.Products.Count());
        }

        [TestMethod]
        // test SemVer filter
        public void Test_Home_Index_Filter_Result()
        {
            string filterString = "2.0.2";   // should return 2.0.2, 2.0.3, 2.1, 2.3, 3, 10.0

            Mock<ISoftwareCatalog> mock = new Mock<ISoftwareCatalog>();
            mock.Setup(m => m.Products).Returns(staticCatalogData().AsQueryable<Software>());

            HomeController hc = new HomeController(mock.Object);

            ViewResult view = (ViewResult)hc.Index(filterString);
            SoftwareCatalogViewModel catalog = (SoftwareCatalogViewModel)view.ViewData.Model;
            Assert.AreEqual(6, catalog.Products.Count());
        }

        [TestMethod]
        // invalid filter, clear the filter, display error result
        public void Test_Home_Index_invalidFilter()
        {
            string filterString = "test";
            Mock<ISoftwareCatalog> mock = new Mock<ISoftwareCatalog>();
            mock.Setup(m => m.Products).Returns(staticCatalogData().AsQueryable<Software>());

            HomeController hc = new HomeController(mock.Object);

            ViewResult view = (ViewResult)hc.Index(filterString);
            SoftwareCatalogViewModel catalog = (SoftwareCatalogViewModel)view.ViewData.Model;
            Assert.AreEqual("",catalog.SoftwareVersionFilter);
            Assert.AreEqual(1, catalog.Products.Count());
            string ExString = $"Error reading (\"{filterString}\"). Try again";
            Assert.AreEqual(ExString, catalog.Products.FirstOrDefault().Name);
            Assert.AreEqual("", catalog.Products.FirstOrDefault().Version);
        }

        [TestMethod]
        // valid filter, no matches found
        public void Test_Home_Index_no_matches()
        {
            string filterString = "999"; 

            Mock<ISoftwareCatalog> mock = new Mock<ISoftwareCatalog>();
            mock.Setup(m => m.Products).Returns(staticCatalogData().AsQueryable<Software>());

            HomeController hc = new HomeController(mock.Object);

            ViewResult view = (ViewResult)hc.Index(filterString);
            SoftwareCatalogViewModel catalog = (SoftwareCatalogViewModel)view.ViewData.Model;
            Assert.AreEqual(1, catalog.Products.Count());
            Assert.AreEqual("No results found", catalog.Products.FirstOrDefault().Name);
            Assert.AreEqual("", catalog.Products.FirstOrDefault().Version);
        }
    }
}
