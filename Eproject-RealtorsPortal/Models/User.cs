using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        [DisplayName("ID")]
        public long UsersId { get; set; }

        [Column("users_fullname")]
        [DisplayName("Full Name")]
        [StringLength(100)]
        [Unicode(false)]
        public string UsersFullname { get; set; } = null!;

        [DisplayName("Email")]
        [Column("users_email")]
        [StringLength(100)]
        [Unicode(false)]
        public string UsersEmail { get; set; } = null!;

        [DisplayName("Confirm Email")]
        [Column("users_confirmEmail")]
        public string ConfirmEmail { get; set; }

        [DisplayName("Phone")]
        [Column("users_phone")]
        [StringLength(13, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 9)]
        [Unicode(false)]
        public string? UsersPhone { get; set; }

        [DisplayName("Address")]
        [Column("users_address")]
        [StringLength(150)]
        [Unicode(false)]
        public string? UsersAddress { get; set; }

        [DisplayName("Gender")]
        [Column("users_gender")]
        public bool? UsersGender { get; set; } = null;

        [DisplayName("Password")]
        [Column("users_password")]
        [StringLength(100)]
        [Unicode(false)]
        public string UsersPassword { get; set; } = null!;

        [Column("users_image")]
        [StringLength(100)]
        [Unicode(false)]
        [DisplayName("Image")]
        public string? UsersImage { get; set; } = "defaultImage.jpg";

        [DisplayName("Status")]
        [Column("users_status")]
        public bool UsersStatus { get; set; } = false;

        [Column("packages_id")]
        public long? PackagesId { get; set; } = 2; // con rang buoc khoa ngoai xu li sau mac dinh 1 gia tri co san de demo (gia tri tai khoan mac dinh la free)

        [ForeignKey("PackagesId")]
        [InverseProperty("Users")]

        public virtual Package Packages { get; set; } = null;
        [InverseProperty("Users")]
        public virtual ICollection<Contact> Contacts { get; set; }
        [InverseProperty("Users")]
        public virtual ICollection<Payment> Payments { get; set; }
        [InverseProperty("Users")]
        public virtual ICollection<Product> Products { get; set; }



        // infor 
        public User GetUser(long id)
        {
            LQHVContext DbContext = new LQHVContext();

            User user = DbContext.Users.Where(u => u.UsersId == id).FirstOrDefault();
            return user;

        }

        //update info
        public User ChangeInfor(long UsersId, string UsersFullname, string UsersPhone, string UsersAddress, bool? UsersGender, string fileName)
        {
            using (var connect = new LQHVContext())
            {
                User user = connect.Users.Where(u => u.UsersId == UsersId).FirstOrDefault();

                if (fileName != null)
                {
                    user.UsersImage = fileName;
                }
                user.UsersFullname = UsersFullname;
                user.UsersPhone = UsersPhone;
                user.UsersAddress = UsersAddress;
                user.UsersGender = UsersGender;
                connect.Users.Update(user);
                if (connect.SaveChanges() >= 1)
                {
                    return user;
                }
                else
                {
                    user = connect.Users.Find(UsersId);
                    return user;
                }
            }
        }

        //Change Password
        public bool ChangePassword(long UsersId, string oldPassword, string newPassword, string ConfirmNewPassword)
        {
            using (var connect = new LQHVContext())
            {
                if (UsersId == 0 || oldPassword == null || newPassword == null || ConfirmNewPassword == null)
                {
                    return false;
                }
                if (newPassword != ConfirmNewPassword)
                {
                    return false;
                }
                User user = connect.Users.Where(u => u.UsersId == UsersId).FirstOrDefault();

                if (oldPassword == user.UsersPassword)
                {
                    user.UsersPassword = newPassword;
                    connect.Users.Update(user);
                    if (connect.SaveChanges() >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
