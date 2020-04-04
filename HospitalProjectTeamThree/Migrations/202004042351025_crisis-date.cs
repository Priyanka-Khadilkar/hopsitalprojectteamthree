namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crisisdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Crises", "CrisisFinished", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Crises", "CrisisFinished", c => c.DateTime(nullable: false));
        }
    }
}
