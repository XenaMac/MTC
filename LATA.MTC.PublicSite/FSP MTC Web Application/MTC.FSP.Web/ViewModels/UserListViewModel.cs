using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    //public class UserListViewModel : ApplicationUser
    //{
    //    public List<RoleViewModel> Roles { get; set; }

    //    [DisplayName("User Role"), Required]
    //    public String SelectedRoleId { get; set; }

    //    public String SelectedRoleName { get; set; }

    //    [DisplayName("User Name"), Required]
    //    public String LogonName { get; set; }

    //    public String UserId { get; set; }

    //    //optional
    //    public String ContractorTypeName { get; set; }

    //    public UserListViewModel()
    //    {
    //        this.Roles = new List<RoleViewModel>();
    //    }

    //    public UserListViewModel(ApplicationUser user)
    //        : this()
    //    {
    //        this.UserName = user.UserName;
    //        this.LogonName = user.UserName;
    //        this.FirstName = user.FirstName;
    //        this.LastName = user.LastName;
    //        this.NickName = user.NickName;
    //        this.PhoneNumber = user.PhoneNumber;
    //        this.Mobile = user.Mobile;
    //        this.Email = user.Email;
    //        this.UserId = user.Id;
    //        this.ContractorId = user.ContractorId;

    //        //get role(s)
    //        using (ApplicationDbContext identity = new ApplicationDbContext())
    //        {
    //            var allRoles = identity.ApplicationRoles.ToList();
    //            foreach (var role in allRoles)
    //            {
    //                var rvm = new RoleViewModel(role);
    //                this.Roles.Add(rvm);
    //            }

    //            if (user.Roles.Any())
    //            {
    //                var rl = this.Roles.Find(r => r.RoleId == user.Roles.FirstOrDefault().RoleId);
    //                this.SelectedRoleId = rl.RoleId;
    //                this.SelectedRoleName = rl.RoleName;
    //            }
    //        }
    //    }

    //}

    //// Used to display a single role with a checkbox, within a list structure:
    //public class RoleViewModel
    //{
    //    public RoleViewModel() { }
    //    public RoleViewModel(IdentityRole role)
    //    {
    //        this.RoleName = role.Name;
    //        this.RoleId = role.Id;
    //    }


    //    [Required]
    //    public String RoleId { get; set; }

    //    public string RoleName { get; set; }
    //}

    public class UserListViewModel
    {       
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String NickName { get; set; }
        
        public String PhoneName { get; set; }

        public String Mobile { get; set; }

        public String Email { get; set; }

        public String PhoneNumber { get; set; }

        public Guid? ContractorId { get; set; }


        public List<RoleViewModel> Roles { get; set; }

        [DisplayName("User Role"), Required]
        public String SelectedRoleId { get; set; }

        public String SelectedRoleName { get; set; }

        [DisplayName("User Name"), Required]
        public String UserName { get; set; }

        public String UserId { get; set; }

        //optional
        public String ContractorTypeName { get; set; }

        public UserListViewModel()
        {
            this.Roles = new List<RoleViewModel>();
        }

        public UserListViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.NickName = user.NickName;
            this.PhoneNumber = user.PhoneNumber;
            this.Mobile = user.Mobile;
            this.Email = user.Email;
            this.UserId = user.Id;
            this.ContractorId = user.ContractorId;

            //get role(s)
            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                var allRoles = identity.ApplicationRoles.ToList();
                foreach (var role in allRoles)
                {
                    var rvm = new RoleViewModel(role);
                    this.Roles.Add(rvm);
                }

                if (user.Roles.Any())
                {
                    var rl = this.Roles.Find(r => r.RoleId == user.Roles.FirstOrDefault().RoleId);
                    this.SelectedRoleId = rl.RoleId;
                    this.SelectedRoleName = rl.RoleName;
                }
            }
        }

    }

    // Used to display a single role with a checkbox, within a list structure:
    public class RoleViewModel
    {
        public RoleViewModel() { }
        public RoleViewModel(IdentityRole role)
        {
            this.RoleName = role.Name;
            this.RoleId = role.Id;
        }


        [Required]
        public String RoleId { get; set; }

        public string RoleName { get; set; }
    }
}