using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class Contact
    {
        [Key]
        [Column("contacts_id")]
        public long ContactsId { get; set; }
        [Column("contacts_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string ContactsName { get; set; } = null!;
        [Column("contacts_phone")]
        [StringLength(20)]
        [Unicode(false)]
        public string? ContactsPhone { get; set; }
        [Column("contacts_email")]
        [StringLength(100)]
        [Unicode(false)]
        public string ContactsEmail { get; set; } = null!;
        [Column("contacts_content")]
        [StringLength(1000)]
        [Unicode(false)]
        public string ContactsContent { get; set; } = null!;
        [Column("users_id")]
        public long? UsersId { get; set; }

        [ForeignKey("UsersId")]
        [InverseProperty("Contacts")]
        public virtual User? Users { get; set; }
    }
}
