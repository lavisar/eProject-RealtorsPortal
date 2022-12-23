using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("category_id")]
        public long CategoryId { get; set; }
        [Column("category_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string CategoryName { get; set; } = null!;
        [Required]
        [Column("category_status")]
        public bool? CategoryStatus { get; set; }
        [Column("businessTypes_id")]
        public long BusinessTypesId { get; set; }

        [ForeignKey("BusinessTypesId")]
        [InverseProperty("Categories")]
        public virtual BusinessType BusinessTypes { get; set; } = null!;
        [InverseProperty("Category")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
