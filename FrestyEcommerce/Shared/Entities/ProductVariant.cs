using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FrestyEcommerce.Shared.Entities
{
    public class ProductVariant
    {
        [JsonIgnore]
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
      
        public int ProductId { get; set; }
        [ForeignKey("ProductTypeId")]
        public ProductType? ProductType { get; set; }
      
        public int ProductTypeId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
