namespace MySwitch.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Acquirer", c => c.String());
            AddColumn("dbo.Transactions", "Issuer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Issuer");
            DropColumn("dbo.Transactions", "Acquirer");
        }
    }
}
