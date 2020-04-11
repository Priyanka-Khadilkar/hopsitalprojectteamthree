namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventremovedEventTimefromdatatypechange : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "EventFromTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "EventFromTime", c => c.String());
        }
    }
}
