namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackDepartmentMedicalstaffdirectory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackFormId = c.Int(nullable: false, identity: true),
                        DoctorName = c.String(),
                        FeedbackType = c.String(),
                        Hygiene = c.String(),
                        StaffKnowledge = c.String(),
                        WaitTime = c.String(),
                        Comments = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FeedbackFormId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MedicalStaffDirectories",
                c => new
                    {
                        MedicalStaffDirectoryId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MedicalStaffDirectoryId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalStaffDirectories", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MedicalStaffDirectories", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Feedbacks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.MedicalStaffDirectories", new[] { "UserId" });
            DropIndex("dbo.MedicalStaffDirectories", new[] { "DepartmentId" });
            DropIndex("dbo.Feedbacks", new[] { "UserId" });
            DropTable("dbo.MedicalStaffDirectories");
            DropTable("dbo.Feedbacks");
        }
    }
}
