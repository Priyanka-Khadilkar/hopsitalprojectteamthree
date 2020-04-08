namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingRoomDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "RoomDesc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "RoomDesc");
        }
    }
}
