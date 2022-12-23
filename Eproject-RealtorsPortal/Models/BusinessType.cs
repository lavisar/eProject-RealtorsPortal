using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class BusinessType
    {
        public BusinessType()
        {
            Categories = new HashSet<Category>();
        }

        [Key]
        [Column("businessTypes_id")]
        public long BusinessTypesId { get; set; }
        [Column("businessTypes_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string BusinessTypesName { get; set; } = null!;

        [InverseProperty("BusinessTypes")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}
