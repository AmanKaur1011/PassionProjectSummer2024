namespace PassionProjectSummer2024.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPreviousDepartmentIdColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "PreviousDepartmentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "PreviousDepartmentId");
        }
    }
}
