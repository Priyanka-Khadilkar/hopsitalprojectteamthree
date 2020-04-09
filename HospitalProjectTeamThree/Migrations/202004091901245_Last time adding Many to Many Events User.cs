namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LasttimeaddingManytoManyEventsUser : DbMigration
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
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.AspNetUsers", t => t.EventCreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.EventUpdatedBy)
                .Index(t => t.EventCreatedBy)
                .Index(t => t.EventUpdatedBy);
            
            CreateTable(
                "dbo.AspNetUserEvents",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.EventId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserEvents", "EventId", "dbo.Events");
            DropForeignKey("dbo.AspNetUserEvents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "EventUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "EventCreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserEvents", new[] { "EventId" });
            DropIndex("dbo.AspNetUserEvents", new[] { "UserId" });
            DropIndex("dbo.Events", new[] { "EventUpdatedBy" });
            DropIndex("dbo.Events", new[] { "EventCreatedBy" });
            DropTable("dbo.AspNetUserEvents");
            DropTable("dbo.Events");
        }
    }
}
