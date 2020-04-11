namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class volunteerposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VolunteerPositions",
                c => new
                    {
                        VolunteerPositionID = c.Int(nullable: false, identity: true),
                        VolunteerPositionTitle = c.String(),
                        VolunteerPositionDescription = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VolunteerPositionID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.VolunteerPositionApplicationUsers",
                c => new
                    {
                        VolunteerPosition_VolunteerPositionID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.VolunteerPosition_VolunteerPositionID, t.ApplicationUser_Id })
                .ForeignKey("dbo.VolunteerPositions", t => t.VolunteerPosition_VolunteerPositionID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.VolunteerPosition_VolunteerPositionID)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolunteerPositionApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.VolunteerPositionApplicationUsers", "VolunteerPosition_VolunteerPositionID", "dbo.VolunteerPositions");
            DropForeignKey("dbo.VolunteerPositions", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.VolunteerPositionApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.VolunteerPositionApplicationUsers", new[] { "VolunteerPosition_VolunteerPositionID" });
            DropIndex("dbo.VolunteerPositions", new[] { "DepartmentID" });
            DropTable("dbo.VolunteerPositionApplicationUsers");
            DropTable("dbo.VolunteerPositions");
        }
    }
}
