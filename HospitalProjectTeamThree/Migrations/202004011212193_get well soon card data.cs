namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class getwellsooncarddata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GetWellSoonCards", "PatientName", c => c.String());
            AddColumn("dbo.GetWellSoonCards", "RoomNumber", c => c.String());
            AddColumn("dbo.GetWellSoonCards", "PatientEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GetWellSoonCards", "PatientEmail");
            DropColumn("dbo.GetWellSoonCards", "RoomNumber");
            DropColumn("dbo.GetWellSoonCards", "PatientName");
        }
    }
}
