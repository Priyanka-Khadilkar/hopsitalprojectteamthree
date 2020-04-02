namespace HospitalProjectTeamThree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carddesigntable : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CardDesigns");
        }
    }
}
