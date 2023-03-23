using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("ward")]
    public partial class Ward
    {
        public Ward()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("district_id")]
        public long DistrictId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("type")]
        [StringLength(10)]
        public string Type { get; set; }

        [ForeignKey(nameof(DistrictId))]
        [InverseProperty("Ward")]
        public virtual District District { get; set; }
        [InverseProperty("Ward")]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
