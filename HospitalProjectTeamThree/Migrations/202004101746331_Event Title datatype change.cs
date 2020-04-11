namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventTitledatatypechange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "EventTitle", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "EventTitle", c => c.Int(nullable: false));
        }
    }
}
