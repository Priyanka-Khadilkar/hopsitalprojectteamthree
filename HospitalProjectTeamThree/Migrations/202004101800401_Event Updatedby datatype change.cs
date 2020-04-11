namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUpdatedbydatatypechange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventFromTime", c => c.String());
            AlterColumn("dbo.Events", "EventUpdatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "EventUpdatedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Events", "EventFromTime");
        }
    }
}
