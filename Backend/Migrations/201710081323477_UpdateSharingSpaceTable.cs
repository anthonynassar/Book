namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSharingSpaceTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SharingSpaces", "Verified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SharingSpaces", "Verified");
        }
    }
}
