namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingRoomBookingfunctionality : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoomApplicationUsers",
                c => new
                    {
                        Room_RoomID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Room_RoomID, t.ApplicationUser_Id })
                .ForeignKey("dbo.Rooms", t => t.Room_RoomID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Room_RoomID)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoomApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RoomApplicationUsers", "Room_RoomID", "dbo.Rooms");
            DropIndex("dbo.RoomApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.RoomApplicationUsers", new[] { "Room_RoomID" });
            DropTable("dbo.RoomApplicationUsers");
        }
    }
}
