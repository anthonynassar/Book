namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        SharingSpaceId = c.String(nullable: false, maxLength: 128),
                        DimensionId = c.String(nullable: false, maxLength: 128),
                        ConstraintId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SharingSpaceId, t.DimensionId, t.ConstraintId })
                .ForeignKey("dbo.Constraints", t => t.ConstraintId, cascadeDelete: true)
                .ForeignKey("dbo.Dimensions", t => t.DimensionId, cascadeDelete: true)
                .ForeignKey("dbo.SharingSpaces", t => t.SharingSpaceId, cascadeDelete: true)
                .Index(t => t.SharingSpaceId)
                .Index(t => t.DimensionId)
                .Index(t => t.ConstraintId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "SharingSpaceId", "dbo.SharingSpaces");
            DropForeignKey("dbo.Events", "DimensionId", "dbo.Dimensions");
            DropForeignKey("dbo.Events", "ConstraintId", "dbo.Constraints");
            DropIndex("dbo.Events", new[] { "ConstraintId" });
            DropIndex("dbo.Events", new[] { "DimensionId" });
            DropIndex("dbo.Events", new[] { "SharingSpaceId" });
            DropTable("dbo.Events");
        }
    }
}
