namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedthedatatypeofOnlineAppointmentBookingStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OnlineAppointmentBookings", "OnlineAppointmentBookingStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OnlineAppointmentBookings", "OnlineAppointmentBookingStatus", c => c.String());
        }
    }
}
