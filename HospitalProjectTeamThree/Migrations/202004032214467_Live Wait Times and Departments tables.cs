namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiveWaitTimesandDepartmentstables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.LiveWaitTimes",
                c => new
                    {
                        WaitUpdateId = c.Int(nullable: false, identity: true),
                        DateandTime = c.DateTime(nullable: false),
                        CurrentWaitTime = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WaitUpdateId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
            AddColumn("dbo.DoctorsBlogs", "BlogDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LiveWaitTimes", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.LiveWaitTimes", new[] { "DepartmentId" });
            DropColumn("dbo.DoctorsBlogs", "BlogDate");
            DropTable("dbo.LiveWaitTimes");
            DropTable("dbo.Departments");
        }
    }
}
