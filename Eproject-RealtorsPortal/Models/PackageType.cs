using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class PackageType
    {
        public PackageType()
        {
            Packages = new HashSet<Package>();
        }

        [Key]
        [Column("packageType_id")]
        public long PackageTypeId { get; set; }
        [Column("packageType_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string PackageTypeName { get; set; } = null!;

        [InverseProperty("PackageType")]
        public virtual ICollection<Package> Packages { get; set; }
    }
}
