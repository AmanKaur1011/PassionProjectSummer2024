namespace PassionProjectSummer2024.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreviousPositionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "PreviousPositionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "PreviousPositionId");
        }
    }
}
