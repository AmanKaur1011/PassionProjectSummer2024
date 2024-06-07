namespace PassionProjectSummer2024.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoofEmployees : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Departments", "NoOfEmployees");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Departments", "NoOfEmployees", c => c.Int(nullable: false));
        }
    }
}
