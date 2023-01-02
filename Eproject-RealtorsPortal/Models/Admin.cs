using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eproject_RealtorsPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace Eproject_RealtorsPortal.Models
{
    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        [Column("admin_id")]
        [Display(Name ="ID")]
        public long AdminId { get; set; }

        [Column("admin_name")]
        [Display(Name = "Name")]
        [StringLength(100)]
        [Unicode(false)]
        public string AdminName { get; set; } = null!;

        [Display(Name = "Email")]
        [Column("admin_email")]
        [StringLength(100)]
        [Unicode(false)]
        public string AdminEmail { get; set; } = null!;

        [DisplayName("Confirm Email")]
        [Column("admin_confirmEmail")]
        public string ConfirmEmail { get; set; } 

        [Column("admin_password")]
        [Display(Name = "Password")]
        [StringLength(100)]
        [Unicode(false)]
        public string AdminPassword { get; set; } = null!;

        [Display(Name = "Image")]
        [Column("admin_image")]
        [StringLength(100)]
        [Unicode(false)]
        public string? AdminImage { get; set; } = "defaultImage.jpg";

        [Column("admin_role")]
        [Display(Name = "Role")]
        [StringLength(1)]
        [Unicode(false)]
        public string AdminRole { get; set; } = "Staff";

        [Required]
        [Column("admin_status")]
        [Display(Name = "Status")]
        public bool AdminStatus { get; set; } = false;


        public Admin ChangeInfor(long AdminId, string AdminName, string fileName, string AdminRole, bool AdminStatus)
        {
            using (var connect = new LQHVContext())
            {
                Admin admin = connect.Admins.Where(u => u.AdminId == AdminId).FirstOrDefault();

                if (fileName != null)
                {
                    admin.AdminImage = fileName;
                }
                admin.AdminName = AdminName;
                admin.AdminStatus = AdminStatus;
                admin.AdminRole = AdminRole;
                connect.Admins.Update(admin);
                if (connect.SaveChanges() >= 1)
                {
                    return admin;
                }
                else
                {
                    admin = connect.Admins.Find(AdminId);
                    return admin;
                }
            }
        }

        public bool ChangePassword(long AdminId, string oldPassword, string newPassword, string ConfirmNewPassword)
        {
            using (var connect = new LQHVContext())
            {
                if (AdminId == 0 || oldPassword == null || newPassword == null || ConfirmNewPassword == null)
                {
                    return false;
                }
                if (newPassword != ConfirmNewPassword)
                {
                    return false;
                }
                Admin admin = connect.Admins.Where(u => u.AdminId == AdminId).FirstOrDefault();

                if (oldPassword == admin.AdminPassword)
                {
                    admin.AdminPassword = newPassword;
                    connect.Admins.Update(admin);
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
