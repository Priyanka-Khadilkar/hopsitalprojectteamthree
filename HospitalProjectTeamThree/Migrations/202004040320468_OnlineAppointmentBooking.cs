namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnlineAppointmentBooking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnlineAppointmentBookings",
                c => new
                    {
                        OnlineAppointmentBookingId = c.Int(nullable: false, identity: true),
                        PatientDateOfBirth = c.DateTime(nullable: false),
                        PreferredDate = c.DateTime(nullable: false),
                        PreferredTime = c.String(),
                        PreferredDoctor = c.String(),
                        OnlineAppointmentBookingStatus = c.String(),
                        OnlineAppointmentBookingBookedOn = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OnlineAppointmentBookingId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OnlineAppointmentBookings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.OnlineAppointmentBookings", new[] { "UserId" });
            DropTable("dbo.OnlineAppointmentBookings");
        }
    }
}
