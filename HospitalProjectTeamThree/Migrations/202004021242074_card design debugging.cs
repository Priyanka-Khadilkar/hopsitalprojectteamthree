namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carddesigndebugging : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GetWellSoonCards", "CardDesignNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GetWellSoonCards", "CardDesignNumber");
        }
    }
}
