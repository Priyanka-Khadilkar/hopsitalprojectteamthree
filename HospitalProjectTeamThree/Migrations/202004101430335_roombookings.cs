namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roombookings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoomBookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        RoomID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoomID);
            
            AddColumn("dbo.Rooms", "RoomBooking_BookingId", c => c.Int());
            CreateIndex("dbo.Rooms", "RoomBooking_BookingId");
            AddForeignKey("dbo.Rooms", "RoomBooking_BookingId", "dbo.RoomBookings", "BookingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoomBookings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rooms", "RoomBooking_BookingId", "dbo.RoomBookings");
            DropForeignKey("dbo.RoomBookings", "RoomID", "dbo.Rooms");
            DropIndex("dbo.RoomBookings", new[] { "RoomID" });
            DropIndex("dbo.RoomBookings", new[] { "UserId" });
            DropIndex("dbo.Rooms", new[] { "RoomBooking_BookingId" });
            DropColumn("dbo.Rooms", "RoomBooking_BookingId");
            DropTable("dbo.RoomBookings");
        }
    }
}
