namespace BabyInABagServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Product_Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Product_Description", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Product_Image", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Product_Image", c => c.String());
            AlterColumn("dbo.Products", "Product_Description", c => c.String());
            AlterColumn("dbo.Products", "Product_Name", c => c.String());
        }
    }
}
