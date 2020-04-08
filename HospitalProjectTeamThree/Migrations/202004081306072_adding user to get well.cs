namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingusertogetwell : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GetWellSoonCards", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.GetWellSoonCards", "User_Id");
            AddForeignKey("dbo.GetWellSoonCards", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GetWellSoonCards", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GetWellSoonCards", new[] { "User_Id" });
            DropColumn("dbo.GetWellSoonCards", "User_Id");
        }
    }
}
