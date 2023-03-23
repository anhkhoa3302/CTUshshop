using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("transaction")]
    public partial class Transaction
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("user_id")]
        [StringLength(450)]
        public string UserId { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Required]
        [Column("code")]
        [StringLength(450)]
        public string Code { get; set; }
        [Required]
        [Column("type")]
        [StringLength(10)]
        public string Type { get; set; }
        [Column("status")]
        public byte? Status { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updatedAt", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty("Transaction")]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUsers.Transaction))]
        public virtual AspNetUsers User { get; set; }
    }
}
