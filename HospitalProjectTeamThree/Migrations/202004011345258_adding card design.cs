namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingcarddesign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GetWellSoonCards", "CardDesignNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GetWellSoonCards", "CardDesignNumber");
        }
    }
}
