using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class News
    {
        [Key]
        [Column("news_id")]
        public long NewsId { get; set; }
        [Column("news_title")]
        [StringLength(300)]
        [Unicode(false)]
        public string NewsTitle { get; set; } = null!;
        [Column("news_content")]
        [Unicode(false)]
        public string NewsContent { get; set; } = null!;
        [Column("news_desc")]
        [StringLength(300)]
        [Unicode(false)]
        public string NewsDesc { get; set; } = null!;
        [Column("news_date", TypeName = "datetime")]
        public DateTime NewsDate { get; set; }
        [Column("news_image")]
        [StringLength(150)]
        [Unicode(false)]
        public string? NewsImage { get; set; }
    }
}
