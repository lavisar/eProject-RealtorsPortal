using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eproject_RealtorsPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    public partial class User
    {
        public User()
        {
            Contacts = new HashSet<Contact>();
            Payments = new HashSet<Payment>();
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("users_id")]
        public long UsersId { get; set; }
        [Column("users_fullname")]
        [StringLength(100)]
        [Unicode(false)]
        public string UsersFullname { get; set; } = null!;
        [Column("users_email")]
        [StringLength(100)]
        [Unicode(false)]
        public string UsersEmail { get; set; } = null!;
        [Column("users_phone")]
        [StringLength(20)]
        [Unicode(false)]
        public string? UsersPhone { get; set; }
        [Column("users_address")]
        [StringLength(150)]
        [Unicode(false)]
        public string? UsersAddress { get; set; }
        [Column("users_gender")]
        public bool? UsersGender { get; set; }
        [Column("users_password")]
        [StringLength(100)]
        [Unicode(false)]
        public string UsersPassword { get; set; } = null!;
        [Column("users_image")]
        [StringLength(100)]
        [Unicode(false)]
        public string? UsersImage { get; set; }
        [Required]
        [Column("users_status")]
        public bool? UsersStatus { get; set; }
        [Column("packages_id")]
        public long PackagesId { get; set; }

        //public bool Remember { get; set; }

        [ForeignKey("PackagesId")]
        [InverseProperty("Users")]
        public virtual Package Packages { get; set; } = null!;
        [InverseProperty("Users")]
        public virtual ICollection<Contact> Contacts { get; set; }
        [InverseProperty("Users")]
        public virtual ICollection<Payment> Payments { get; set; }
        [InverseProperty("Users")]
        public virtual ICollection<Product> Products { get; set; }




        ////code xu li
        //public User Login(string userEmail, string userPassword)
        //{
        //    using (var connect = new LQHVContext())
        //    {

        //        try
        //        {
        //            User user = new User();
        //            user = (Customer)connect.Customers.Single(c => c.Email == email);
        //            if (cus.Email != null)
        //            {
        //                if (password == cus.Password)
        //                {
        //                    cus.LastLoggedTime = DateTime.Now;
        //                    connect.Customers.Update(cus);
        //                    connect.SaveChanges();
        //                }
        //                return cus;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        catch (System.Exception)
        //        {

        //            return null;
        //        }
        //    }
        //}
    }
}
