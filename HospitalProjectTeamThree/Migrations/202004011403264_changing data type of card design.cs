namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingdatatypeofcarddesign : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GetWellSoonCards", "CardDesignNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GetWellSoonCards", "CardDesignNumber", c => c.Int(nullable: false));
        }
    }
}
