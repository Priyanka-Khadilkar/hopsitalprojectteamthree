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
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //One user can create many get well cards
        public virtual ICollection<GetWellSoonCard> GetWellSoonCards { get; set; }

        //One user can book many Appointments.
        public virtual ICollection<OnlineAppointmentBooking> OnlineAppointmentBookings { get; set; }

        //One user can write many Job Listings
        public virtual ICollection<JobListing> JobListings { get; set; }
        //One user may have many Bookings
        public virtual ICollection<Room> Rooms { get; set; }

        //Representing the Many in (Many Events to Many Users)
        public virtual ICollection<Event> Events { get; set; }

        //many users to many volunteer positions
        public virtual ICollection<VolunteerPosition> VolunteerPositions { get; set; }
    }
    //adding roles to user: Admin, Editor, Registered user
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }

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


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Done by : Priyanka Khadilkar
            //Overriding the On model creating for many to many relationship with Application User 
            //Representing the Many in (Many Events to Many Users)
            modelBuilder.Entity<ApplicationUser>()
                        .HasMany<Event>(s => s.Events)
                        .WithMany(c => c.Users)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("UserId");
                            cs.MapRightKey("EventId");
                            cs.ToTable("AspNetUserEvents");
                        });

            base.OnModelCreating(modelBuilder);
        }



        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.GetWellSoonCard> GetWellSoonCards { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.Room> Rooms { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.DoctorsBlog> DoctorsBlogs { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.BlogTopic> Topics { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.LiveWaitTime> LiveWaitTimes { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.Department> Departments { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.Article> Articles { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.CardDesign> CardDesigns { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.Crisis> Crisiss { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.OnlineAppointmentBooking> OnlineAppointmentBookings { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.JobListing> JobListings { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.Event> Events { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.RoomBooking> RoomBookings { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.Feedback> Feedbacks { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.MedicalStaffDirectory> MedicalStaffDirectories { get; set; }
        public System.Data.Entity.DbSet<HospitalProjectTeamThree.Models.VolunteerPosition> VolunteerPositions { get; set; }
      
    }
}