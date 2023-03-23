using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("report_category")]
    public partial class ReportCategory
    {
        public ReportCategory()
        {
            Report = new HashSet<Report>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("title")]
        [StringLength(256)]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }

        [InverseProperty("ReportCategory")]
        public virtual ICollection<Report> Report { get; set; }
    }
}
