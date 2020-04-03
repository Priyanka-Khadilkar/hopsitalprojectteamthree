namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crisis : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Crises",
                c => new
                    {
                        CrisisId = c.Int(nullable: false, identity: true),
                        CrisisName = c.String(),
                        CrisisDesc = c.String(),
                        CrisisStrated = c.DateTime(nullable: false),
                        CrisisFinished = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CrisisId);
            
            AddColumn("dbo.Articles", "Crisis_CrisisId", c => c.Int());
            CreateIndex("dbo.Articles", "Crisis_CrisisId");
            AddForeignKey("dbo.Articles", "Crisis_CrisisId", "dbo.Crises", "CrisisId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "Crisis_CrisisId", "dbo.Crises");
            DropIndex("dbo.Articles", new[] { "Crisis_CrisisId" });
            DropColumn("dbo.Articles", "Crisis_CrisisId");
            DropTable("dbo.Crises");
        }
    }
}
