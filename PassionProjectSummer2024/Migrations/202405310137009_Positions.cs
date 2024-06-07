namespace PassionProjectSummer2024.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Positions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        PositionTitle = c.String(),
                        HourlyWage = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PositionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Positions");
        }
    }
}
