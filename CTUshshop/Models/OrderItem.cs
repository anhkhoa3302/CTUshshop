using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("order_item")]
    public partial class OrderItem
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("product_id")]
        public long ProductId { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Column("quantity")]
        public double Quantity { get; set; }
        [Column("totalPrice")]
        public double TotalPrice { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty("OrderItem")]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("OrderItem")]
        public virtual Product Product { get; set; }
    }
}
