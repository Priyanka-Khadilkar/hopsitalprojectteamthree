namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEventTablewithtimefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventFromTime", c => c.String());
            AddColumn("dbo.Events", "EventToTime", c => c.String());
            DropColumn("dbo.Events", "EventTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "EventTime", c => c.String());
            DropColumn("dbo.Events", "EventToTime");
            DropColumn("dbo.Events", "EventFromTime");
        }
    }
}
