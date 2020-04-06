namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobListing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobListings",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(),
                        JobDescription = c.String(),
                        Salary = c.Double(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Published = c.Boolean(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JobID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.DepartmentID)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobListings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobListings", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.JobListings", new[] { "UserId" });
            DropIndex("dbo.JobListings", new[] { "DepartmentID" });
            DropTable("dbo.JobListings");
        }
    }
}
