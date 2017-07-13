namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserTodoTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "UserId", c => c.String());
            AlterColumn("dbo.Users", "Address", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Address", c => c.Long());
            DropColumn("dbo.TodoItems", "UserId");
        }
    }
}
