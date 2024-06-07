namespace PassionProjectSummer2024.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesmade : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "favColor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "favColor", c => c.String());
        }
    }
}
