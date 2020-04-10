namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roombookinsfixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomBookings", "PaymentCleared", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoomBookings", "DateFrom", c => c.String());
            AddColumn("dbo.RoomBookings", "DateTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomBookings", "DateTo");
            DropColumn("dbo.RoomBookings", "DateFrom");
            DropColumn("dbo.RoomBookings", "PaymentCleared");
        }
    }
}
