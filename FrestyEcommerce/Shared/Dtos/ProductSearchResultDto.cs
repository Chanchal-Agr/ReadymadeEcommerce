using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrestyEcommerce.Shared.Entities;

namespace FrestyEcommerce.Shared.Dtos
{
    public class ProductSearchResultDto
    {
        public List<Product> Products { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
