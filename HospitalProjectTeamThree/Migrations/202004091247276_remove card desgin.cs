namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removecarddesgin : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GetWellSoonCards", "CardDesignNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GetWellSoonCards", "CardDesignNumber", c => c.String());
        }
    }
}
