namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class joblistingsalarychage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobListings", "Salary", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobListings", "Salary", c => c.Double(nullable: false));
        }
    }
}
