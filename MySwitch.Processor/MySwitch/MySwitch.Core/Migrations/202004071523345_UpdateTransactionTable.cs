namespace MySwitch.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTransactionTable : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Transactions", "DateCreated");
            //DropColumn("dbo.Transactions", "DateModified");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Transactions", "DateModified", c => c.DateTime(nullable: false));
            //AddColumn("dbo.Transactions", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
