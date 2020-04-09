namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userstodoctorstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DoctorsBlogs", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DoctorsBlogs", "User_Id");
            AddForeignKey("dbo.DoctorsBlogs", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoctorsBlogs", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DoctorsBlogs", new[] { "User_Id" });
            DropColumn("dbo.DoctorsBlogs", "User_Id");
        }
    }
}
