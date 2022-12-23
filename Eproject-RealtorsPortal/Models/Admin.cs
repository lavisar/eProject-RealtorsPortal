using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        [Column("admin_id")]
        public long AdminId { get; set; }
        [Column("admin_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string AdminName { get; set; } = null!;
        [Column("admin_email")]
        [StringLength(100)]
        [Unicode(false)]
        public string AdminEmail { get; set; } = null!;
        [Column("admin_password")]
        [StringLength(100)]
        [Unicode(false)]
        public string AdminPassword { get; set; } = null!;
        [Column("admin_image")]
        [StringLength(100)]
        [Unicode(false)]
        public string? AdminImage { get; set; }
        [Column("admin_role")]
        [StringLength(1)]
        [Unicode(false)]
        public string AdminRole { get; set; } = null!;
        [Required]
        [Column("admin_status")]
        public bool? AdminStatus { get; set; }
    }
}
