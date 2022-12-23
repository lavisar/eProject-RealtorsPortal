using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class Area
    {
        [Key]
        [Column("areas_id")]
        public long AreasId { get; set; }
        [Column("areas_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string AreasName { get; set; } = null!;
        [Column("cities_id")]
        public long CitiesId { get; set; }

        [ForeignKey("CitiesId")]
        [InverseProperty("Areas")]
        public virtual City Cities { get; set; } = null!;
    }
}
