using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class Region
    {
        public Region()
        {
            Cities = new HashSet<City>();
        }

        [Key]
        [Column("regions_id")]
        public long RegionsId { get; set; }
        [Column("regions_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string RegionsName { get; set; } = null!;
        [Column("countries_id")]
        public long CountriesId { get; set; }

        [ForeignKey("CountriesId")]
        [InverseProperty("Regions")]
        public virtual Country Countries { get; set; } = null!;
        [InverseProperty("Regions")]
        public virtual ICollection<City> Cities { get; set; }
    }
}
