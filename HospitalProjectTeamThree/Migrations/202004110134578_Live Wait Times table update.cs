namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiveWaitTimestableupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LiveWaitTimes", "WaitUpdateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.LiveWaitTimes", "WaitUpdateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.LiveWaitTimes", "DateandTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LiveWaitTimes", "DateandTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.LiveWaitTimes", "WaitUpdateTime");
            DropColumn("dbo.LiveWaitTimes", "WaitUpdateDate");
        }
    }
}
