namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateObjectTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Objects", "Uri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Objects", "Uri");
        }
    }
}
