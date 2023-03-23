using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            Message = new HashSet<Message>();
            Order = new HashSet<Order>();
            Product = new HashSet<Product>();
            Report = new HashSet<Report>();
            Transaction = new HashSet<Transaction>();
        }

        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [Column("firstName")]
        [StringLength(10)]
        public string FirstName { get; set; }
        [Column("middleName")]
        [StringLength(10)]
        public string MiddleName { get; set; }
        [Column("lastName")]
        [StringLength(15)]
        public string LastName { get; set; }
        [Column("address")]
        [StringLength(200)]
        public string Address { get; set; }
        [Column("province_id")]
        public long ProvinceId { get; set; }
        [Column("district_id")]
        public long DistrictId { get; set; }
        [Column("ward_id")]
        public long WardId { get; set; }
        [Column("profile_pic")]
        [StringLength(256)]
        public string ProfilePic { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("status")]
        public byte? Status { get; set; }

        [ForeignKey(nameof(DistrictId))]
        [InverseProperty("AspNetUsers")]
        public virtual District District { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("AspNetUsers")]
        public virtual Province Province { get; set; }
        [ForeignKey(nameof(WardId))]
        [InverseProperty("AspNetUsers")]
        public virtual Ward Ward { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Message> Message { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Order> Order { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Product> Product { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Report> Report { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
