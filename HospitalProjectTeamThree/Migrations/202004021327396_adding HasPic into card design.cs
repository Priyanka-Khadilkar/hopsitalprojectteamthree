namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingHasPicintocarddesign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardDesigns", "HasPic", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardDesigns", "HasPic");
        }
    }
}
