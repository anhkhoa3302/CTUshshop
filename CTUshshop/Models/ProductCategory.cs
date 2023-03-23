using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("product_category")]
    public partial class ProductCategory
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("product_id")]
        public long ProductId { get; set; }
        [Column("category_id")]
        public long CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("ProductCategory")]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductCategory")]
        public virtual Product Product { get; set; }
    }
}
