using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eproject_RealtorsPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            Images = new HashSet<Image>();
            Payments = new HashSet<Payment>();
        }

        [Key]
        [Column("product_id")]
        public long ProductId { get; set; }
        [Column("product_title")]
        [StringLength(100)]
        [Unicode(false)]
        public string ProductTitle { get; set; } = null!;
        [Column("product_desc")]
        [StringLength(2000)]
        [Unicode(false)]
        public string ProductDesc { get; set; } = null!;
        [Column("product_area", TypeName = "decimal(18, 2)")]
        public decimal ProductArea { get; set; }
        [Column("product_price", TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; }
        [Column("product_image")]
        [StringLength(200)]
        [Unicode(false)]
        public string ProductImage { get; set; } = null!;
        [Column("product_address")]
        [StringLength(200)]
        [Unicode(false)]
        public string ProductAddress { get; set; } = null!;
        [Column("product_legal")]
        [StringLength(50)]
        [Unicode(false)]
        public string? ProductLegal { get; set; }
        [Column("product_interior")]
        [StringLength(50)]
        [Unicode(false)]
        public string? ProductInterior { get; set; }
        [Column("num_toilets")]
        public int? NumToilets { get; set; }
        [Column("num_bedrooms")]
        public int? NumBedrooms { get; set; }
        [Column("home_orientation")]
        [StringLength(50)]
        [Unicode(false)]
        public string? HomeOrientation { get; set; }
        [Column("balcony_orientation")]
        [StringLength(50)]
        [Unicode(false)]
        public string? BalconyOrientation { get; set; }
        [Column("num_ofFloors")]
        public int? NumOfFloors { get; set; }
        [Column("packages_id")]
        public long PackagesId { get; set; }
        [Column("num_days")]
        public int NumDays { get; set; }
        [Column("start_date", TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column("end_date", TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [Column("featured")]
        public bool Featured { get; set; }
        [Column("category_id")]
        public long CategoryId { get; set; }
        [Column("contactName")]
        [StringLength(100)]
        [Unicode(false)]
        public string ContactName { get; set; } = null!;
        [Column("phoneNumber")]
        [StringLength(20)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        [Column("contactAddress")]
        [StringLength(100)]
        [Unicode(false)]
        public string? ContactAddress { get; set; }
        [Column("contactEmail")]
        [StringLength(100)]
        [Unicode(false)]
        public string ContactEmail { get; set; } = null!;
        [Column("users_id")]
        public long? UsersId { get; set; }
        [Column("status")]
        [StringLength(20)]
        [Unicode(false)]
        public string Status { get; set; } = null!;

        [ForeignKey("CategoryId")]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; } = null!;
        [ForeignKey("PackagesId")]
        [InverseProperty("Products")]
        public virtual Package Packages { get; set; } = null!;
        [ForeignKey("UsersId")]
        [InverseProperty("Products")]
        public virtual User? Users { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Image> Images { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Payment> Payments { get; set; }




        //LQHVContext LQHVContext = new LQHVContext();
        //BusinessType BusinessTypeID;
        //public long GetBusinessTypeID(Product model)
        //{
        //    BusinessTypeID = LQHVContext.BusinessTypes.Find(Category.BusinessTypes.BusinessTypesId);
        //    return BusinessTypeID.BusinessTypesId;
        //}
    }
    public class ProductBox
    {
        public long ProductID { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductArea { get; set; }
        public string ProductAddress { get; set; }
        public long BusinessTypeID { get; set; }
    }
    public class ProductDetail
    {
        public long ProductID { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDesc { get; set; } = null!;
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductArea { get; set; }
        public string ProductAddress { get; set; }
        public long BusinessTypeID { get; set; }
        public string? ProductLegal { get; set; }
        public string? ProductInterior { get; set; }
        public int? NumToilets { get; set; }
        public int? NumBedrooms { get; set; }
        public string? HomeOrientation { get; set; }
        public string? BalconyOrientation { get; set; }
        public int? NumOfFloors { get; set; }
        public string ContactName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ContactAddress { get; set; }
        public string ContactEmail { get; set; } = null!;

    }
}
