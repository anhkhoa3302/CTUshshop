using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("evidence")]
    public partial class Evidence
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("report_id")]
        public long ReportId { get; set; }
        [Required]
        [Column("evid_string")]
        public string EvidString { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(ReportId))]
        [InverseProperty("Evidence")]
        public virtual Report Report { get; set; }
    }
}
