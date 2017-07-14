namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyDimension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dimensions", "Label", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dimensions", "Label");
        }
    }
}
