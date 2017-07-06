namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationObjectSharingspace : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Objects", "UserId", c => c.Long(nullable: false));
            AddColumn("dbo.Objects", "SharingSpaceId", c => c.Long(nullable: false));
            AddColumn("dbo.Objects", "SharingSpace_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.SharingSpaces", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.Objects", "SharingSpace_Id");
            AddForeignKey("dbo.Objects", "SharingSpace_Id", "dbo.SharingSpaces", "Id");
            DropColumn("dbo.Objects", "IdObject");
            DropColumn("dbo.Objects", "IdUser");
            DropColumn("dbo.Objects", "IdSs");
            DropColumn("dbo.SharingSpaces", "IdSs");
            DropColumn("dbo.SharingSpaces", "IdUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SharingSpaces", "IdUser", c => c.Long(nullable: false));
            AddColumn("dbo.SharingSpaces", "IdSs", c => c.Long(nullable: false));
            AddColumn("dbo.Objects", "IdSs", c => c.Long(nullable: false));
            AddColumn("dbo.Objects", "IdUser", c => c.Long(nullable: false));
            AddColumn("dbo.Objects", "IdObject", c => c.Long(nullable: false));
            DropForeignKey("dbo.Objects", "SharingSpace_Id", "dbo.SharingSpaces");
            DropIndex("dbo.Objects", new[] { "SharingSpace_Id" });
            DropColumn("dbo.SharingSpaces", "UserId");
            DropColumn("dbo.Objects", "SharingSpace_Id");
            DropColumn("dbo.Objects", "SharingSpaceId");
            DropColumn("dbo.Objects", "UserId");
        }
    }
}
