using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalProjectTeamThree.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalProjectTeamThree.Data
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //PetGroomingContext class has been adjusted to become a subclass of IdentityDbContext instead of DbContext
    //Why? Because This class helps support base login functionality (IdentityDbContext).
    public class HospitalProjectTeamThreeContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public HospitalProjectTeamThreeContext() : base("name=HospitalProjectTeamThreeContext")
        {
        }
        public static HospitalProjectTeamThreeContext Create()
        {
            return new HospitalProjectTeamThreeContext();
        }

        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.GetWellSoonCard> GetWellSoonCards { get; set; }

       }
}