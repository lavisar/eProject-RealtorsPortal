using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class Country
    {
        public Country()
        {
            Regions = new HashSet<Region>();
        }

        [Key]
        [Column("countries_id")]
        public long CountriesId { get; set; }
        [Column("countries_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string CountriesName { get; set; } = null!;

        [InverseProperty("Countries")]
        public virtual ICollection<Region> Regions { get; set; }
    }
}
