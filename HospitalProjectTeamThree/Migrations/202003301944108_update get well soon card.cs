namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updategetwellsooncard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GetWellSoonCards",
                c => new
                    {
                        CardId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        HasPic = c.Int(nullable: false),
                        PicExt = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CardId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GetWellSoonCards", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.GetWellSoonCards", new[] { "UserId" });
            DropTable("dbo.GetWellSoonCards");
        }
    }
}
