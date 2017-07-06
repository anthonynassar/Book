namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSharingspace : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Objects", "SharingSpaceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Objects", "SharingSpaceId", c => c.Long(nullable: false));
        }
    }
}
