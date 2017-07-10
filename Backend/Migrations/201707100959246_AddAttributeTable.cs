namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttributeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        ObjectId = c.String(nullable: false, maxLength: 128),
                        DatatypeId = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.ObjectId, t.DatatypeId })
                .ForeignKey("dbo.Datatypes", t => t.DatatypeId, cascadeDelete: true)
                .ForeignKey("dbo.Objects", t => t.ObjectId, cascadeDelete: true)
                .Index(t => t.ObjectId)
                .Index(t => t.DatatypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attributes", "ObjectId", "dbo.Objects");
            DropForeignKey("dbo.Attributes", "DatatypeId", "dbo.Datatypes");
            DropIndex("dbo.Attributes", new[] { "DatatypeId" });
            DropIndex("dbo.Attributes", new[] { "ObjectId" });
            DropTable("dbo.Attributes");
        }
    }
}
