namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResolvedEventconflictissue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Event_EventId" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Event_EventId", c => c.Int());
            DropForeignKey("dbo.AspNetUserEvents", "EventId", "dbo.Events");
            DropForeignKey("dbo.AspNetUserEvents", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserEvents", new[] { "EventId" });
            DropIndex("dbo.AspNetUserEvents", new[] { "UserId" });
            DropTable("dbo.AspNetUserEvents");
            CreateIndex("dbo.Events", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "Event_EventId");
            AddForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events", "EventId");
        }
    }
}
