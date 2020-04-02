namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorsTableupdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DoctorsBlogs", "TopicId", "dbo.BlogTopics");
            AddColumn("dbo.DoctorsBlogs", "BlogTopic_TopicId", c => c.Int());
            AddColumn("dbo.BlogTopics", "DoctorsBlog_BlogId", c => c.Int());
            CreateIndex("dbo.DoctorsBlogs", "BlogTopic_TopicId");
            CreateIndex("dbo.BlogTopics", "DoctorsBlog_BlogId");
            AddForeignKey("dbo.BlogTopics", "DoctorsBlog_BlogId", "dbo.DoctorsBlogs", "BlogId");
            AddForeignKey("dbo.DoctorsBlogs", "BlogTopic_TopicId", "dbo.BlogTopics", "TopicId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoctorsBlogs", "BlogTopic_TopicId", "dbo.BlogTopics");
            DropForeignKey("dbo.BlogTopics", "DoctorsBlog_BlogId", "dbo.DoctorsBlogs");
            DropIndex("dbo.BlogTopics", new[] { "DoctorsBlog_BlogId" });
            DropIndex("dbo.DoctorsBlogs", new[] { "BlogTopic_TopicId" });
            DropColumn("dbo.BlogTopics", "DoctorsBlog_BlogId");
            DropColumn("dbo.DoctorsBlogs", "BlogTopic_TopicId");
            AddForeignKey("dbo.DoctorsBlogs", "TopicId", "dbo.BlogTopics", "TopicId", cascadeDelete: true);
        }
    }
}
