namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class events_users_many_to_many_removed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events");
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Event_EventId" });
            DropColumn("dbo.Events", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "Event_EventId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Event_EventId", c => c.Int());
            AddColumn("dbo.Events", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Event_EventId");
            CreateIndex("dbo.Events", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events", "EventId");
            AddForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
