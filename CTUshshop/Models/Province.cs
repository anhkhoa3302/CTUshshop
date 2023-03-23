using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("province")]
    public partial class Province
    {
        public Province()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            District = new HashSet<District>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("type")]
        [StringLength(10)]
        public string Type { get; set; }

        [InverseProperty("Province")]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        [InverseProperty("Province")]
        public virtual ICollection<District> District { get; set; }
    }
}
