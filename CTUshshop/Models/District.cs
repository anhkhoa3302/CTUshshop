using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("district")]
    public partial class District
    {
        public District()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            Ward = new HashSet<Ward>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("province_id")]
        public long ProvinceId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("type")]
        [StringLength(10)]
        public string Type { get; set; }

        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("District")]
        public virtual Province Province { get; set; }
        [InverseProperty("District")]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        [InverseProperty("District")]
        public virtual ICollection<Ward> Ward { get; set; }
    }
}
