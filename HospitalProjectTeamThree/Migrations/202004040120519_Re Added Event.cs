namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReAddedEvent : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.AspNetUsers", t => t.EventCreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.EventUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.EventCreatedBy)
                .Index(t => t.EventUpdatedBy)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.AspNetUsers", "Event_EventId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Event_EventId");
            AddForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "EventUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "EventCreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Event_EventId", "dbo.Events");
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Events", new[] { "EventUpdatedBy" });
            DropIndex("dbo.Events", new[] { "EventCreatedBy" });
            DropIndex("dbo.AspNetUsers", new[] { "Event_EventId" });
            DropColumn("dbo.AspNetUsers", "Event_EventId");
            DropTable("dbo.Events");
        }
    }
}
