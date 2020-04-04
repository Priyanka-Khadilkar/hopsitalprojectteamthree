namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorsBlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorsBlogs",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        BlogTitle = c.String(),
                        BlogContent = c.String(),
                        BlogSource = c.String(),
                        TopicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlogId)
                .ForeignKey("dbo.BlogTopics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.BlogTopics",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        TopicName = c.String(),
                    })
                .PrimaryKey(t => t.TopicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoctorsBlogs", "TopicId", "dbo.BlogTopics");
            DropIndex("dbo.DoctorsBlogs", new[] { "TopicId" });
            DropTable("dbo.BlogTopics");
            DropTable("dbo.DoctorsBlogs");
        }
    }
}
