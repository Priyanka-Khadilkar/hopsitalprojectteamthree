namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedEvent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "EventCreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "EventUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Event_EventId" });
            DropIndex("dbo.Events", new[] { "EventCreatedBy" });
            DropIndex("dbo.Events", new[] { "EventUpdatedBy" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Event_EventId");
            DropTable("dbo.Events");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventTitle = c.Int(nullable: false),
                        EventStartDate = c.DateTime(nullable: false),
                        EventEndDate = c.DateTime(nullable: false),
                        EventTime = c.String(),
                        EventLocation = c.String(),
                        EventTargetAudience = c.String(),
                        EventHostedBy = c.String(),
                        EventImagePath = c.String(),
                        EventDetails = c.String(),
                        EventCreatedBy = c.String(maxLength: 128),
                        EventUpdatedBy = c.String(maxLength: 128),
                        EventCreatedOn = c.DateTime(nullable: false),
                        EventUpdatedOn = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EventId);
            
            AddColumn("dbo.AspNetUsers", "Event_EventId", c => c.Int());
            CreateIndex("dbo.Events", "ApplicationUser_Id");
            CreateIndex("dbo.Events", "EventUpdatedBy");
            CreateIndex("dbo.Events", "EventCreatedBy");
            CreateIndex("dbo.AspNetUsers", "Event_EventId");
            AddForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Events", "EventUpdatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Events", "EventCreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events", "EventId");
        }
    }
}
