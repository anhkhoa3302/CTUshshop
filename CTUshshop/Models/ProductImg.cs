using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("product_img")]
    public partial class ProductImg
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("product_id")]
        public long ProductId { get; set; }
        [Required]
        [Column("img")]
        [StringLength(256)]
        public string Img { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductImg")]
        public virtual Product Product { get; set; }
    }
}
