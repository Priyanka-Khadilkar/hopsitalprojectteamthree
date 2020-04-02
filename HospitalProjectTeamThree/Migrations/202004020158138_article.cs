namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class article : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        ArticleTitle = c.String(),
                        ArticleAuthor = c.String(),
                        ArticleContent = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Articles");
        }
    }
}
