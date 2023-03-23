using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Models
{
    [Table("report")]
    public partial class Report
    {
        public Report()
        {
            Evidence = new HashSet<Evidence>();
        }

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
        [Column("report_category_id")]
        public long ReportCategoryId { get; set; }
        [Required]
        [Column("summary")]
        [StringLength(250)]
        public string Summary { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(ReportCategoryId))]
        [InverseProperty("Report")]
        public virtual ReportCategory ReportCategory { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUsers.Report))]
        public virtual AspNetUsers User { get; set; }
        [InverseProperty("Report")]
        public virtual ICollection<Evidence> Evidence { get; set; }
    }
}
