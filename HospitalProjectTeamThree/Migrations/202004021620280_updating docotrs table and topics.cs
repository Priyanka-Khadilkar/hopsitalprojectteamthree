namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingdocotrstableandtopics : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DoctorsBlogs", "BlogTopic_TopicId", "dbo.BlogTopics");
            DropForeignKey("dbo.DoctorsBlogs", "TopicId", "dbo.BlogTopics");
            DropForeignKey("dbo.BlogTopics", "DoctorsBlog_BlogId", "dbo.DoctorsBlogs");
            DropIndex("dbo.DoctorsBlogs", new[] { "TopicId" });
            DropIndex("dbo.DoctorsBlogs", new[] { "BlogTopic_TopicId" });
            DropIndex("dbo.BlogTopics", new[] { "DoctorsBlog_BlogId" });
            CreateTable(
                "dbo.BlogTopicDoctorsBlogs",
                c => new
                    {
                        BlogTopic_TopicId = c.Int(nullable: false),
                        DoctorsBlog_BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BlogTopic_TopicId, t.DoctorsBlog_BlogId })
                .ForeignKey("dbo.BlogTopics", t => t.BlogTopic_TopicId, cascadeDelete: true)
                .ForeignKey("dbo.DoctorsBlogs", t => t.DoctorsBlog_BlogId, cascadeDelete: true)
                .Index(t => t.BlogTopic_TopicId)
                .Index(t => t.DoctorsBlog_BlogId);
            
            DropColumn("dbo.DoctorsBlogs", "TopicId");
            DropColumn("dbo.DoctorsBlogs", "BlogTopic_TopicId");
            DropColumn("dbo.BlogTopics", "DoctorsBlog_BlogId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogTopics", "DoctorsBlog_BlogId", c => c.Int());
            AddColumn("dbo.DoctorsBlogs", "BlogTopic_TopicId", c => c.Int());
            AddColumn("dbo.DoctorsBlogs", "TopicId", c => c.Int(nullable: false));
            DropForeignKey("dbo.BlogTopicDoctorsBlogs", "DoctorsBlog_BlogId", "dbo.DoctorsBlogs");
            DropForeignKey("dbo.BlogTopicDoctorsBlogs", "BlogTopic_TopicId", "dbo.BlogTopics");
            DropIndex("dbo.BlogTopicDoctorsBlogs", new[] { "DoctorsBlog_BlogId" });
            DropIndex("dbo.BlogTopicDoctorsBlogs", new[] { "BlogTopic_TopicId" });
            DropTable("dbo.BlogTopicDoctorsBlogs");
            CreateIndex("dbo.BlogTopics", "DoctorsBlog_BlogId");
            CreateIndex("dbo.DoctorsBlogs", "BlogTopic_TopicId");
            CreateIndex("dbo.DoctorsBlogs", "TopicId");
            AddForeignKey("dbo.BlogTopics", "DoctorsBlog_BlogId", "dbo.DoctorsBlogs", "BlogId");
            AddForeignKey("dbo.DoctorsBlogs", "TopicId", "dbo.BlogTopics", "TopicId", cascadeDelete: true);
            AddForeignKey("dbo.DoctorsBlogs", "BlogTopic_TopicId", "dbo.BlogTopics", "TopicId");
        }
    }
}
