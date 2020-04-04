namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeredundantdatagetwellsooncard : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GetWellSoonCards", name: "CardUserId", newName: "CardUsers_Id");
            RenameIndex(table: "dbo.GetWellSoonCards", name: "IX_CardUserId", newName: "IX_CardUsers_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.GetWellSoonCards", name: "IX_CardUsers_Id", newName: "IX_CardUserId");
            RenameColumn(table: "dbo.GetWellSoonCards", name: "CardUsers_Id", newName: "CardUserId");
        }
    }
}
