namespace PassionProjectSummer2024.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FavouriteColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FavColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FavColor");
        }
    }
}
