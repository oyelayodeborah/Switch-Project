namespace MySwitch.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinInst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinancialInstitutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SinkNodeId = c.Int(nullable: false),
                        InstitutionCode = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SinkNodes", t => t.SinkNodeId, cascadeDelete: true)
                .Index(t => t.SinkNodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinancialInstitutions", "SinkNodeId", "dbo.SinkNodes");
            DropIndex("dbo.FinancialInstitutions", new[] { "SinkNodeId" });
            DropTable("dbo.FinancialInstitutions");
        }
    }
}
