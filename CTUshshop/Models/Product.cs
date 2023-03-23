using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("product")]
    public partial class Product
    {
        public Product()
        {
            OrderItem = new HashSet<OrderItem>();
            ProductCategory = new HashSet<ProductCategory>();
            ProductImg = new HashSet<ProductImg>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("user_id")]
        [StringLength(450)]
        public string UserId { get; set; }
        [Required]
        [Column("title")]
        [StringLength(256)]
        public string Title { get; set; }
        [Required]
        [Column("summary")]
        public string Summary { get; set; }
        [Required]
        [Column("content")]
        public string Content { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("quantity")]
        public double? Quantity { get; set; }
        [Required]
        [Column("unit")]
        [StringLength(20)]
        public string Unit { get; set; }
        [Column("status")]
        public byte? Status { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updatedAt", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUsers.Product))]
        public virtual AspNetUsers User { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductImg> ProductImg { get; set; }
    }
}
