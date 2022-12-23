using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class City
    {
        public City()
        {
            Areas = new HashSet<Area>();
        }

        [Key]
        [Column("cities_id")]
        public long CitiesId { get; set; }
        [Column("cities_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string CitiesName { get; set; } = null!;
        [Column("regions_id")]
        public long RegionsId { get; set; }

        [ForeignKey("RegionsId")]
        [InverseProperty("Cities")]
        public virtual Region Regions { get; set; } = null!;
        [InverseProperty("Cities")]
        public virtual ICollection<Area> Areas { get; set; }
    }
}
