namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applypendingchange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobListings", "Published", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobListings", "Published", c => c.Boolean(nullable: false));
        }
    }
}
