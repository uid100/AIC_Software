using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIC_Software.Models.ViewModels
{
    public class SoftwareCatalogViewModel
    {
        public IEnumerable<Software> Products { get; set; }
        public string SoftwareVersionFilter { get; set; }
    }
}
