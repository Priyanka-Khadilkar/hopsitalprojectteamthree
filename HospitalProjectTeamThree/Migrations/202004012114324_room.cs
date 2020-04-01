namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class room : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Int(nullable: false, identity: true),
                        RoomNumber = c.Int(nullable: false),
                        RoomType = c.String(),
                        RoomTotalBeds = c.Int(nullable: false),
                        RoomPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rooms");
        }
    }
}
