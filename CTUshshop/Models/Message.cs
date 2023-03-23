using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("message")]
    public partial class Message
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("user_id")]
        [StringLength(450)]
        public string UserId { get; set; }
        [Required]
        [Column("to_user_id")]
        [StringLength(450)]
        public string ToUserId { get; set; }
        [Required]
        [Column("text")]
        [StringLength(400)]
        public string Text { get; set; }
        [Column("status")]
        public byte? Status { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUsers.Message))]
        public virtual AspNetUsers User { get; set; }
    }
}
