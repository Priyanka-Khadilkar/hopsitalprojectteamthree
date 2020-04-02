namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingtables : DbMigration
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
            
            AddColumn("dbo.GetWellSoonCards", "CardDesignId", c => c.Int(nullable: false));
            CreateIndex("dbo.GetWellSoonCards", "CardDesignId");
            AddForeignKey("dbo.GetWellSoonCards", "CardDesignId", "dbo.CardDesigns", "CardDesignId", cascadeDelete: true);
            DropColumn("dbo.GetWellSoonCards", "CardDesignNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GetWellSoonCards", "CardDesignNumber", c => c.String());
            DropForeignKey("dbo.GetWellSoonCards", "CardDesignId", "dbo.CardDesigns");
            DropIndex("dbo.GetWellSoonCards", new[] { "CardDesignId" });
            DropColumn("dbo.GetWellSoonCards", "CardDesignId");
            DropTable("dbo.CardDesigns");
        }
    }
}
