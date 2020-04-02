namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingcarddesignnameindesigntable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardDesigns", "DesignName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardDesigns", "DesignName");
        }
    }
}
