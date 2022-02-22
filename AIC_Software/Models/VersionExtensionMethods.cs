using System.Collections.Generic;
using System.Management.Automation;

namespace AIC_Software.Models
{
    public static class VersionExtensionMethods
    {
        public static IEnumerable<Software> VersionFilter(this IEnumerable<Software> Products, SemanticVersion oldest)
        {
            List<Software> filteredList = new List<Software>();
            SemanticVersion ver;

            foreach( var product in Products )
            {
                if( SemanticVersion.TryParse(product.Version, out ver) && ver >= oldest)
                {
                    filteredList.Add(product);
                }
            }

            if( filteredList.Count < 1 )
            {
                filteredList.Add(new Software { Name = "No results found", Version = "" });
            }

            return filteredList;
        }
    }
}
