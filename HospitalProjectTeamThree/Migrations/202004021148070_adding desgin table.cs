namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingdesgintable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardDesigns",
                c => new
                    {
                        CardDesignId = c.Int(nullable: false, identity: true),
                        PicExt = c.String(),
                    })
                .PrimaryKey(t => t.CardDesignId);
            
            DropColumn("dbo.GetWellSoonCards", "CardDesignNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GetWellSoonCards", "CardDesignNumber", c => c.String());
            DropTable("dbo.CardDesigns");
        }
    }
}
