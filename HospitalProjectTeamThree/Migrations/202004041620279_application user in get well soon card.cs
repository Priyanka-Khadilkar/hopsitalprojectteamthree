namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationuseringetwellsooncard : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GetWellSoonCards", name: "CardUsers_Id", newName: "Users_Id");
            RenameIndex(table: "dbo.GetWellSoonCards", name: "IX_CardUsers_Id", newName: "IX_Users_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.GetWellSoonCards", name: "IX_Users_Id", newName: "IX_CardUsers_Id");
            RenameColumn(table: "dbo.GetWellSoonCards", name: "Users_Id", newName: "CardUsers_Id");
        }
    }
}
