using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CTUshshop.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column("firstName", TypeName = "nvarchar(10)")]
        public string FirstName { get; set; }
        [PersonalData]
        [Column("middleName", TypeName = "nvarchar(10)")]
        public string MiddleName { get; set; }
        [PersonalData]
        [Column("lastName", TypeName = "nvarchar(15)")]
        public string LastName { get; set; }

        [PersonalData]
        [Column("address", TypeName = "nvarchar(200)")]
        public string Address { get; set; }

        [PersonalData]
        [Column("province_id")]
        public long ProvinceId { get; set; }
        [PersonalData]
        [Column("district_id")]
        public long DistrictId { get; set; }
        [PersonalData]
        [Column("ward_id")]
        public long WardId { get; set; }

    }
}
