namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDimDatatypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DimDatatypes",
                c => new
                    {
                        DimensionId = c.String(nullable: false, maxLength: 128),
                        DatatypeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DimensionId, t.DatatypeId })
                .ForeignKey("dbo.Datatypes", t => t.DatatypeId, cascadeDelete: true)
                .ForeignKey("dbo.Dimensions", t => t.DimensionId, cascadeDelete: true)
                .Index(t => t.DimensionId)
                .Index(t => t.DatatypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DimDatatypes", "DimensionId", "dbo.Dimensions");
            DropForeignKey("dbo.DimDatatypes", "DatatypeId", "dbo.Datatypes");
            DropIndex("dbo.DimDatatypes", new[] { "DatatypeId" });
            DropIndex("dbo.DimDatatypes", new[] { "DimensionId" });
            DropTable("dbo.DimDatatypes");
        }
    }
}
