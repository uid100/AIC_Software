using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AIC_Software.Models;
using AIC_Software.Models.ViewModels;
using System.Management.Automation;

namespace AIC_Software.Controllers
{
    public class HomeController : Controller
    {
        private ISoftwareCatalog catalog;
        public HomeController(ISoftwareCatalog catalogDataSource)
        {
            catalog = catalogDataSource;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string SoftwareVersionFilter)
        {
            IEnumerable<Software> productList;
            SemanticVersion SemVer = new SemanticVersion(0);

            // validate user input
            if ( !SemanticVersion.TryParse(SoftwareVersionFilter, out SemVer) )
            {
                productList = new List<Software>
                {
                    new Software { 
                        Name = $"Error reading (\"{SoftwareVersionFilter}\"). Try again", 
                        Version = "" 
                    }
                };
                SemVer = new SemanticVersion(0);
                SoftwareVersionFilter = "";
            }

            // filter result set
            else
            {
                productList = catalog.Products.VersionFilter(SemVer);
            }

            return View(
                new SoftwareCatalogViewModel()
                {
                    SoftwareVersionFilter = SoftwareVersionFilter, //SemVer.ToString(),
                    Products = productList
                }
            );
        }
    }
}
