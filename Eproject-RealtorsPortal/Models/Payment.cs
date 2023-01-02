using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    [Table("Payment")]
    public partial class Payment
    {
        [Key]
        [Column("payment_id")]
        public long PaymentId { get; set; }
        [Column("payment_price", TypeName = "decimal(18, 2)")]
        public decimal PaymentPrice { get; set; }

        [Column("payment_total", TypeName = "decimal(18, 2)")]
        public decimal PaymentTotal { get; set; }
        [Column("payment_datetime", TypeName = "datetime")]
        public DateTime PaymentDatetime { get; set; }
        [Column("product_id")]
        public long ProductId { get; set; }
        [Column("users_id")]
        public long UsersId { get; set; }
        [Column("payment_status")]
        public bool? PaymentStatus { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Payments")]
        public virtual Product Product { get; set; } = null!;
        [ForeignKey("UsersId")]
        [InverseProperty("Payments")]
        public virtual User Users { get; set; } = null!;
    }
}
