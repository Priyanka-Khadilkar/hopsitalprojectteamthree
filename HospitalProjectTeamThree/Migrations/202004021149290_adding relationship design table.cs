namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingrelationshipdesigntable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GetWellSoonCards", "CardDesignId", c => c.Int());
            CreateIndex("dbo.GetWellSoonCards", "CardDesignId");
            AddForeignKey("dbo.GetWellSoonCards", "CardDesignId", "dbo.CardDesigns", "CardDesignId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GetWellSoonCards", "CardDesignId", "dbo.CardDesigns");
            DropIndex("dbo.GetWellSoonCards", new[] { "CardDesignId" });
            DropColumn("dbo.GetWellSoonCards", "CardDesignId");
        }
    }
}
