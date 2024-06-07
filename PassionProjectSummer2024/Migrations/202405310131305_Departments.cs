namespace PassionProjectSummer2024.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Departments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        DepartmentManager = c.String(),
                        NoOfEmployees = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Departments");
        }
    }
}
