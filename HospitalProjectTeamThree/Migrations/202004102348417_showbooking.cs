namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class showbooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomBookings", "Room_RoomID", c => c.Int());
            CreateIndex("dbo.RoomBookings", "Room_RoomID");
            AddForeignKey("dbo.RoomBookings", "Room_RoomID", "dbo.Rooms", "RoomID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoomBookings", "Room_RoomID", "dbo.Rooms");
            DropIndex("dbo.RoomBookings", new[] { "Room_RoomID" });
            DropColumn("dbo.RoomBookings", "Room_RoomID");
        }
    }
}
