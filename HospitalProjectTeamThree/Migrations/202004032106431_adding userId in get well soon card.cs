namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinguserIdingetwellsooncard : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GetWellSoonCards", "User_Id", "dbo.AspNetUsers");
            AddColumn("dbo.GetWellSoonCards", "CardUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.GetWellSoonCards", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.GetWellSoonCards", "CardUserId");
            CreateIndex("dbo.GetWellSoonCards", "ApplicationUser_Id");
            AddForeignKey("dbo.GetWellSoonCards", "CardUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GetWellSoonCards", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GetWellSoonCards", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GetWellSoonCards", "CardUserId", "dbo.AspNetUsers");
            DropIndex("dbo.GetWellSoonCards", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GetWellSoonCards", new[] { "CardUserId" });
            DropColumn("dbo.GetWellSoonCards", "ApplicationUser_Id");
            DropColumn("dbo.GetWellSoonCards", "CardUserId");
            AddForeignKey("dbo.GetWellSoonCards", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
