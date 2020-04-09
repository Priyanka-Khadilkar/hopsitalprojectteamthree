namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class droppinguseringetwellcard : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GetWellSoonCards", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GetWellSoonCards", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GetWellSoonCards", "Users_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GetWellSoonCards", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GetWellSoonCards", new[] { "User_Id" });
            DropIndex("dbo.GetWellSoonCards", new[] { "Users_Id" });
            DropColumn("dbo.GetWellSoonCards", "ApplicationUser_Id");
            //DropColumn("dbo.GetWellSoonCards", "User_Id");
           // DropColumn("dbo.GetWellSoonCards", "Users_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GetWellSoonCards", "Users_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.GetWellSoonCards", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.GetWellSoonCards", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.GetWellSoonCards", "Users_Id");
            CreateIndex("dbo.GetWellSoonCards", "User_Id");
            CreateIndex("dbo.GetWellSoonCards", "ApplicationUser_Id");
            AddForeignKey("dbo.GetWellSoonCards", "Users_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GetWellSoonCards", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GetWellSoonCards", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
