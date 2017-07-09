namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInterestTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        PreferenceId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.PreferenceId })
                .ForeignKey("dbo.Preferences", t => t.PreferenceId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PreferenceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interests", "UserId", "dbo.Users");
            DropForeignKey("dbo.Interests", "PreferenceId", "dbo.Preferences");
            DropIndex("dbo.Interests", new[] { "PreferenceId" });
            DropIndex("dbo.Interests", new[] { "UserId" });
            DropTable("dbo.Interests");
        }
    }
}
