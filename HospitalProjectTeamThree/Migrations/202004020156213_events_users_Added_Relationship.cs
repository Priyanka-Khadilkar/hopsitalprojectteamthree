namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class events_users_Added_Relationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Event_EventId", c => c.Int());
            CreateIndex("dbo.Events", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "Event_EventId");
            AddForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Event_EventId" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Event_EventId");
            DropColumn("dbo.Events", "ApplicationUser_Id");
        }
    }
}
