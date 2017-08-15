namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Username", c => c.String());
            AddColumn("dbo.Users", "GivenName", c => c.String());
            AddColumn("dbo.Users", "Surname", c => c.String());
            AddColumn("dbo.Users", "Birthdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Gender", c => c.String());
            AddColumn("dbo.Users", "CultureInfo", c => c.String());
            AddColumn("dbo.Users", "Country", c => c.String());
            AddColumn("dbo.Users", "City", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 40));
            CreateIndex("dbo.Users", "Email");
            DropColumn("dbo.Users", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Name", c => c.String());
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Users", "Email", c => c.String());
            DropColumn("dbo.Users", "City");
            DropColumn("dbo.Users", "Country");
            DropColumn("dbo.Users", "CultureInfo");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "Birthdate");
            DropColumn("dbo.Users", "Surname");
            DropColumn("dbo.Users", "GivenName");
            DropColumn("dbo.Users", "Username");
        }
    }
}
