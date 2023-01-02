using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class Package
    {
        public Package()
        {
            Products = new HashSet<Product>();
            Users = new HashSet<User>();
            Payments = new HashSet<Payment>();
        }

        [Key]
        [Column("packages_id")]
        public long PackagesId { get; set; }
        [Column("packages_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string PackagesName { get; set; } = null!;
        [Column("packages_duration")]
        public int PackagesDuration { get; set; }
        [Column("packages_price", TypeName = "decimal(18, 2)")]
        public decimal PackagesPrice { get; set; }
        [Column("packages_desc")]
        [StringLength(500)]
        [Unicode(false)]
        public string PackagesDesc { get; set; } = null!;
        [Column("packageType_id")]
        public long PackageTypeId { get; set; }
        [Required]
        [Column("packages_status")]
        public bool? PackagesStatus { get; set; }

        [ForeignKey("PackageTypeId")]
        [InverseProperty("Packages")]
        public virtual PackageType PackageType { get; set; } = null!;
        [InverseProperty("Packages")]
        public virtual ICollection<Product> Products { get; set; }
        [InverseProperty("Packages")]
        public virtual ICollection<User> Users { get; set; }

        [InverseProperty("Package")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
