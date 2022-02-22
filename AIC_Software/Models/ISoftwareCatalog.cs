using System.Collections.Generic;

namespace AIC_Software.Models
{
    public interface ISoftwareCatalog
    {
        IEnumerable<Software> Products { get; }
        IEnumerable<Software> GetAllSoftware();
    }
}
