using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("order")]
    public partial class Order
    {
        public Order()
        {
            OrderItem = new HashSet<OrderItem>();
            Transaction = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("user_id")]
        [StringLength(450)]
        public string UserId { get; set; }
        [Required]
        [Column("firstName")]
        [StringLength(10)]
        public string FirstName { get; set; }
        [Required]
        [Column("middleName")]
        [StringLength(10)]
        public string MiddleName { get; set; }
        [Required]
        [Column("lastName")]
        [StringLength(15)]
        public string LastName { get; set; }
        [Required]
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [Column("phoneNumber")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        [Required]
        [Column("address")]
        [StringLength(300)]
        public string Address { get; set; }
        [Column("tax")]
        public double Tax { get; set; }
        [Column("shipping")]
        public double Shipping { get; set; }
        [Required]
        [Column("shipping_via")]
        [StringLength(20)]
        public string ShippingVia { get; set; }
        [Column("total")]
        public double Total { get; set; }
        [Column("grandTotal")]
        public double GrandTotal { get; set; }
        [Column("status")]
        public byte? Status { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updatedAt", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUsers.Order))]
        public virtual AspNetUsers User { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
