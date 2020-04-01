namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removinghaspicandpicextfromgetwellsooncard : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GetWellSoonCards", "HasPic");
            DropColumn("dbo.GetWellSoonCards", "PicExt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GetWellSoonCards", "PicExt", c => c.String());
            AddColumn("dbo.GetWellSoonCards", "HasPic", c => c.Int(nullable: false));
        }
    }
}
